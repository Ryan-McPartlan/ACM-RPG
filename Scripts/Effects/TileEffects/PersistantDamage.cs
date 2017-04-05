using UnityEngine;
using System.Collections;

public class PersistantDamage : TileEffect
{
    public int damageAmount;

    protected override void OnTick()
    {
        for(int i = 0; i < affectedTile.unitsOnTile.Count; i++)
        {
            affectedTile.unitsOnTile[i].TakeDamage(damageAmount);
        }
        base.OnTick();
    }
}
