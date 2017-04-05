using UnityEngine;
using System.Collections;

public class DamagePickup : TileEffect
{
    public int damageAmount;

    public override void OnCharacterEnter(Unit unit)
    {
        unit.TakeDamage(damageAmount);
        OnRemove();
    }
}
