using UnityEngine;

public class Ability : MonoBehaviour {
    
    public string abilityName;
    public string description;
    public Sprite sprite;
    public int abilityID;

    public bool targeted;
    public float optimalRange;
    public float maxRange;
    public float coolDown;
    public float readyTime;

    public int cost;

    //When we attempt to use an ability
    public virtual void OnUse(Unit caster, Tile target = null)
    {
        //Make extra sure our ability is ready to be used
        if (caster.currentMana > cost && Time.time > readyTime)
        {
            caster.SpendMana(cost);
            readyTime = Time.time + (coolDown * caster.CooldownReduction);
            UseAbility(caster, target);
        }
    }

    //The thing our ability does
    protected virtual void UseAbility(Unit caster, Tile target = null)
    {

    }

}
