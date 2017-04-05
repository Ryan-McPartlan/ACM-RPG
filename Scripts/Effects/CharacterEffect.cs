using UnityEngine;
using System.Collections.Generic;


//To create a new character effect, create a script that inherits from this, add it to an empty object
//Use the editor to setup an effectName, type, and declare whethere it is negative
//Declare a description in start, or ovveride the made description function for HTML stuff like colored text
//Use ovverides for different functionality. Temporary buffs like "5 strength" can be added onapply, and removed onremove
//Make this gameObject a prefab, and create a referance to this prefab by a unit, ability, projectile, or tileEffect that applies this effect
//Use unit.addeffect on the target unit

//The effect applier gets to specify the number of ticks and duration when they call addeffect
public abstract class CharacterEffect : MonoBehaviour {
    
    public bool negativeEffect;
    
    public string effectName;
    public string description;
    public string type;
    public Sprite sprite;

    public int startTicks;
    public int ticksRemaining; //How many times will we tick before falling off. Make -1 for permanenet effect
    public float nextTickTime; //When will we tick next. Set by time + tickDelay after each tick
    public float tickDelay; //How long to wait between ticks
    public Unit unitAffected; //What unit is this effect applied to
    public Unit caster; //If someone made this effect, who

    protected virtual void Start()
    {
        startTicks = ticksRemaining;
        nextTickTime = Time.time + tickDelay;
    }

    //If a given effect is permanent and does not tick, it can override update and not call the base
    protected virtual void Update()
    {
        //If it is time to tick, call ontick, reduce ticks by one, reset tick timer
        if(Time.time > nextTickTime)
        {
            nextTickTime = Time.time + tickDelay;
            OnTick();
            ticksRemaining -= 1;
        }

        //If we are finished, call finish and remove
        if(ticksRemaining == 0)
        {
            OnFinish();
            OnRemove();
        }
    }

    //Called when the effect is applied to the character
    public virtual void OnApply() {
        if(unitAffected = PlayerController.playerController.currentPlayer)
        {
            UIController.uIController.AddBuffToBar(this);
        }
    }

    //Called when unit takes damage, but before unit checks if its dead. Returns a new damage
    public virtual int OnTakeDamage(int amount)
    {
        return amount;
    }
    //Called when unit takes healing, as a negative number. Returns a new amount if changed.
    public virtual int OnHeal(int amount)
    {
        return amount;
    }

    //Called when we heal over maxHealth
    public virtual void OnOverHeal(int amount)
    {
    }

    //Called when we lose mana
    public virtual int OnSpendMana(int amount)
    {
        return amount;
    }

    //Called when we gain mana
    public virtual int OnGainMana(int amount)
    {
        return amount;
    }

    //Called when we gain mana over the limit
    public virtual void OnOverMana(int amount)
    {

    }

    //Called at each tick
    protected virtual void OnTick() { }

    //Called when the effect is removed by any means. When ovveridden, we MUST call base.OnRemove at the end.
    public virtual void OnRemove()
    {
        unitAffected.effects.Remove(this);
        Destroy(gameObject);
    }

    //Called when the effect finished naturally
    protected virtual void OnFinish() { }

    public virtual string GetDescription()
    {
        return description;
    }
}
