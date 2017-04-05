using UnityEngine;
using System.Collections;

public class Flight : CharacterEffect {
    
	public override void OnApply()
    {
        base.OnApply();
        unitAffected.flying = true;
    }

    public override void OnRemove()
    {
        unitAffected.flying = false;
        base.OnRemove();
    }

    public override int OnTakeDamage(int amount)
    {
        OnRemove();
        return amount;
    }
}
