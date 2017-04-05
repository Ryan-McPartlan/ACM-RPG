using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public static PlayerController playerController;

    public Unit mainPlayer;
    public Unit currentPlayer;

    //For abilities
    [SerializeField]
    Ability[] equiptAbilities = new Ability[6]; //Our 6 slots
    [SerializeField]
    GameObject[] activeUI = new GameObject[6]; //If we set these active, it indicates the ability is active
    [SerializeField]
    RectTransform[] CDdisplayes = new RectTransform[6]; //If we set these active, it indicates the ability is active
    [SerializeField]
    Text[] CDtext = new Text[6]; //CDtext
    [SerializeField]
    Image[] abilitySprites = new Image[6]; //If we set these active, it indicates the ability is active

    //Item and equiptment abilities. The controlling player has the 3rd list, only main player has access to these lists
    public List<Ability> itemAbilities = new List<Ability>();
    public List<Ability> equiptmentAbilities = new List<Ability>();

    int activeAbility = -1;
    public int ActiveAbility
    {
        get
        {
            return activeAbility;
        }
        set
        {
            activeAbility = value;

            for (int i = 0; i < 6; i++)
            {
                activeUI[i].SetActive(false);
            }

            if(value != -1)
            {
                activeUI[value].SetActive(true);
            }
        }
    }

    public Text healthText;
    public Text manaText;
    public Slider healthBar;
    public Slider manaBar;
    
    void Awake()
    {
        if (playerController == null)
        {
            playerController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    
    //
    void Update()
    { 
        //Update health and mana bar TODO: OPTOMIZE: Call when updated or not every frame
        healthText.text = currentPlayer.currentHealth.ToString() + "/" + currentPlayer.MaxHealth.ToString();
        manaText.text = currentPlayer.currentMana.ToString() + "/" + currentPlayer.MaxMana.ToString();
        healthBar.maxValue = currentPlayer.MaxHealth;
        manaBar.maxValue = currentPlayer.MaxMana;
        healthBar.value = currentPlayer.currentHealth;
        manaBar.value = currentPlayer.currentMana;

        //Update the cooldownBars
        for(int i = 0; i< 6; i++)
        {
            if(equiptAbilities[i] != null)
            {
                float timeTillReady = Mathf.Max(equiptAbilities[i].readyTime - Time.time, 0);
                float percentComplete = timeTillReady / equiptAbilities[i].coolDown;

                CDdisplayes[i].sizeDelta = new Vector2(60, 60 * percentComplete);
                if (timeTillReady > 0)
                {
                    CDtext[i].text = ((int)(Mathf.Floor(timeTillReady) + 1)).ToString();
                }
                else
                {
                    CDtext[i].text = "";
                }
            }
        }
    }

    //Spawn the player somewhere
    public void Spawn(Tile tile)
    {
        currentPlayer.CurrentTile = tile;
        currentPlayer.transform.position = tile.transform.position;
    }

    //If we have an active ability, use it on the tile. If we have no active ability, walk. TODO: move to a good range if we are out of range
    public void TileClicked(Tile tile)
    {
        if(ActiveAbility != -1 && Vector3.Distance(tile.transform.position, currentPlayer.transform.position) < equiptAbilities[ActiveAbility].maxRange)
        {
            equiptAbilities[ActiveAbility].OnUse(currentPlayer, tile);
            ActiveAbility = -1;
        }
        else if(!tile.blocksMovement || (currentPlayer.flying && !tile.blocksFliers))
        {
            GivePath(tile);
        }
    }

    public void GivePath(Tile tile)
    {
        currentPlayer.targetTile = tile;
        currentPlayer.path.Clear();
    }

    //Called when the player pushed one of the 6 ability buttons
    public void UseAbility(int abilityNumber)
    {
        //If there is no ability in this slot, return null
        if (equiptAbilities[abilityNumber] == null)
        {
            return;
        }
        if (equiptAbilities[abilityNumber].readyTime < Time.time)
        {
            //If the ability is not targeted, cast it. Otherwise, set it active. The next tile click will activate it
            if (!equiptAbilities[abilityNumber].targeted)
            {
                equiptAbilities[abilityNumber].OnUse(currentPlayer);
                ActiveAbility = -1;
            }
            else
            {
                ActiveAbility = abilityNumber;
            }
        }
    }

    public void EquiptAbility(int slot, Ability ability)
    {
        equiptAbilities[slot] = ability;
        abilitySprites[slot].sprite = ability.sprite;
    }
}
