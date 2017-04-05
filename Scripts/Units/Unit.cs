using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    //Stat scaling
    static int primaryScale = 5;
    static int secondaryScale = 3;
    static float damageScale = 0.01f;
    static float resistanceScale = 0.003f;
    static float cDRScale = 0.003f;
    static int hPScale = 5;
    static float hPRegenScale = 0.2f;
    static int manaScale = 5;
    static float manaRegenScale = 1f;
    static float critScale = 0.01f;
    static float baseCritDamage = 1.5f;
    static float critDamageScale = 0.01f;
    static float speedScale = 0.01f;
    static float speedConstant = 2;
    static float dodgeScale = 0.003f;
    static float findScale = 0.01f;

    HealthBar healthBar;

    public int baseHealth; //Different for each unit!
    public int baseMana; //^^
    public float baseSpeed; //^^
    public float baseManaRegen; //^^ 
    public float baseHealthRegen; //Most NPCs will have very high base regen, but 0 in combat

    //For some AI stuff
    public string faction;
    public string[] enemies;
    public string[] friends;

    //For pathing    
    static float distanceThreshhold = 0.02f; //When we are less than this distance from a target location, we have reached it
    Tile currentTile;
    public Tile CurrentTile
    {
        get { return currentTile; }
        set
        {
            previousTile = currentTile;
            if (currentTile != null)
                currentTile.LeaveTile(this);
            currentTile = value;
            currentTile.EnterTile(this);
        }
    }
    public Tile previousTile; //TODO
    public Tile targetTile;
    public List<Tile> path = new List<Tile>();

    //Movement flags
    public bool moving;
    public bool immobilezed;
    public bool stunned;
    public bool flying;
   
    //Stats
    //Some stats, like CDR, dodge chance, and ailment resist increae in value exponentially per percent: 1% is worth twice as much at 50% and 10x more at 90%. 
    //We use a 0.99^scaling formula, so each "point" of scaling has equal value.
    //These stats should cap at 50%
    public float MeleeMod
    {
        get { return 1 + GetScore(strength, dexterity) * damageScale; } //Linear scaling forever
    }
    public float AilmentResist
    {
        get { return 1 - Mathf.Max(Mathf.Pow(1 - resistanceScale, GetScore(strength, constitution)), 0.5f); } // Will give us a number between 0 - 0.5 to check resists with
    }
    public float MagicMod
    {
        get { return 1 + GetScore(intellect, spirit) * damageScale; } //Linear scaling
    }
    public float CooldownReduction
    {
        get { return Mathf.Max(Mathf.Pow(cDRScale, GetScore(intellect, agility)), 0.5f); } //Will give us a number between 1-.5 to multiply our cooldowns by
    }
    public int MaxHealth
    {
        get { return baseHealth + (GetScore(constitution, strength) + strength + intellect + constitution + spirit + dexterity + agility + luck - 7) * hPScale; } //Linear scaling forever
    }
    public float RegenHealth
    {
        get { return baseHealthRegen + hPRegenScale * GetScore(constitution, spirit); } //
    }
    public int MaxMana
    {
        get { return baseMana + GetScore(spirit, intellect) * manaScale; }
    }
    public float RegenMana { 
        get { return baseManaRegen + manaRegenScale * GetScore(spirit, constitution);}
    }
    public float CritChance
    {
        get { return Mathf.Min(critScale * GetScore(dexterity, luck), 1); }
    }
    public float CritDamage
    {
        get { return baseCritDamage + critDamageScale * GetScore(dexterity, strength); }
    }
    public float MoveSpeed {
        get { return speedConstant * (baseSpeed + GetScore(agility, dexterity) * speedScale); }
    }
    public float DodgeChance
    {
        get { return Mathf.Min(1 - Mathf.Pow(1 - dodgeScale, GetScore(agility, luck)), 0.5f); }
    } //Whenever we take a hit, attempt to dodge it
    public float ItemFind
    {
        get { return 1 + GetScore(luck, intellect) * findScale; }
    } //Only used by player! NPCs have 0 luck. Interacts with item drop tables.
    public float GoldFind
    {
        get { return 1 + GetScore(luck, agility) * findScale; }
    } //Only used by player! NPCs have 0 luck. Each time gold is found, multiply by this
    
    //Used above only to get our scales
    int GetScore(int primaryStat, int secondaryStat)
    {
        //Each stat has a primary and secondary attribute. We subtract by primary/secondary scale so that when stats are 0, score is 0.
        return primaryScale * primaryStat + secondaryScale * secondaryStat - primaryScale - secondaryScale;
    }

    //CurrentHealth/Mana are not quite stats, so I have them seperate 
    public int currentHealth;
    public int currentMana;
    float nextRegen;
    float regenDelay = .1f;
    float leftOverHealth; //Holds leftover health/mana to avoid rounding erros
    float leftOverMana; //^

    //Attributes
    public int strength;      //Modifier for melee
    public int intellect;  //Modifier for magic
    public int constitution;  //Health/regen
    public int spirit;        //Mana/regen
    public int dexterity;     //Modifier for range
    public int agility;       //Movespeed / dodge
    public int luck;

    //For effects
    public List<CharacterEffect> effects = new List<CharacterEffect>();

    //For Combat   
    Weapon equiptedWeapon; //All Units have a weapon, weapons have one ability in them. This basic attack is used every time it is ready and in range of current enemy.
    public List<Ability> abilities = new List<Ability>();

    protected virtual void Start()
    {
        currentHealth = MaxHealth;
        currentMana = MaxMana;

        healthBar = UIController.uIController.CreateHealthBar(gameObject);
        healthBar.UpdateBar(currentHealth, MaxHealth, currentMana, MaxMana);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(path.Count == 0 && targetTile != null && targetTile != currentTile)
            RequestPath();
        TakeStep();

        if (Time.time > nextRegen)
        {
            //This will add the "leftover" from rounding to our next health/mana regen step, so health/mana is not lost to rounding errors during base regen
            int healthToGain = (int) (RegenHealth * regenDelay + leftOverHealth);
            leftOverHealth = (RegenHealth * regenDelay + leftOverHealth) - healthToGain;
            int manaToGain = (int)(RegenMana * regenDelay + leftOverMana);
            leftOverMana = (RegenMana * regenDelay + leftOverMana) - manaToGain;

            TakeDamage(-healthToGain);
            SpendMana(-manaToGain);
            nextRegen = Time.time + regenDelay;
        }
    }

    protected void RequestPath()
    {
        path.Clear();
        path.Add(targetTile);
    }


    //Each frame, we move if we need to
    protected void TakeStep()
    {
        //If we aren't moving right now, and we have a path, start moving
        if (!moving && path.Count > 0)
        {
            moving = true;
            CurrentTile = path[0];
            //Remove this from our path since we use it
            path.RemoveAt(0);
        }
        //If we are close enought to the next tile, arrive there
        if(moving && Vector2.Distance(gameObject.transform.position, currentTile.gameObject.transform.position) < distanceThreshhold)
        {
            //If we are close enough, just make sure we are centered
            gameObject.transform.position = currentTile.gameObject.transform.position;

            //Check to see if we want to keep moving
            if(path.Count > 0)
            {
                CurrentTile = path[0];
                path.RemoveAt(0);
            }
            else
            {
                moving = false;
            }
        }

        //Immobilized stops all movement. If we are stunned, we can move if pushed.
        if (moving && !immobilezed && !stunned)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTile.transform.position, Time.deltaTime * MoveSpeed);
        }
    }

    //Add an effect to our effects list
    public void AddEffect(GameObject effect, Unit caster = null)
    {
        //Instantiate the effect and get the actual effect off the gameobject
        GameObject newEffectObject = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
        newEffectObject.transform.parent = transform;

        CharacterEffect newEffect = newEffectObject.GetComponent<CharacterEffect>();

        //Add the effect to the character, initialize it
        effects.Add(newEffect);
        newEffect.unitAffected = this;
        newEffect.caster = caster;
        newEffect.OnApply();
    }

    //Called when taking healing or damage. Called by effects on the character, area/tile effects, or successfully landed attacks
    public void TakeDamage(int damage)
    {
        //Check all effects that occur on take damage. They will return a new damage amount, but will usually return the given amount unchanged
        if(damage > 0)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                damage = effects[i].OnTakeDamage(damage);
            }
        }
        else if(damage < 0)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                damage = effects[i].OnHeal(damage);
            }
        }


        //DEAL DAMAGE
        //Deal the damage, set the text
        currentHealth -= damage;

        //Set the text, color based on heal/damage
        if(damage >= MaxHealth * 0.01f)
        {
            Color color = Color.red;
            if(damage < 0)
            {
                color = Color.green;
            }

            UIController.uIController.CreateFloatingText(damage.ToString(), transform.position, color);
        }

        healthBar.UpdateBar(currentHealth, MaxHealth, currentMana, MaxMana);

        //AFTER DAMAGE
        //Check all effects for overhealing effects if we overheal
        if(currentHealth > MaxHealth)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].OnOverHeal(currentHealth - MaxHealth);
            }

            //If we are still over healed after these effects, set health to max health
            if(currentHealth > MaxHealth)
            {
                currentHealth = MaxHealth;
            }
        }

        //Dead!
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    //Called when we use mana
    public bool SpendMana(int amount, bool voluntary = true) {

        //If we dont have enough mana, return false if attempting to cast or set mana to zero if we are hit by a mana reducing effect
        if (currentMana < amount)
        {
            if (!voluntary)
            {
                currentMana = 0;
            }
            return false;
        }

        //Check all effects that occur on take damage. They will return a new damage amount, but will usually return the given amount unchanged
        if (amount > 0)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                amount = effects[i].OnSpendMana(amount);
            }
        }
        else
        {
            for (int i = 0; i < effects.Count; i++)
            {
                amount = effects[i].OnGainMana(amount);
            }
        }

        //SPEND THE MANA
        currentMana -= amount;
        healthBar.UpdateBar(currentHealth, MaxHealth, currentMana, MaxMana);
        
        //AFTER SPENDING
        if (currentMana > MaxMana)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].OnOverMana(currentHealth - MaxHealth);
            }

            //If we are still over healed after these effects, set health to max health
            if (currentMana > MaxMana)
            {
                currentMana = MaxMana;
            }
        }

        return true;
    }

    public void Die()
    {

    }
}
