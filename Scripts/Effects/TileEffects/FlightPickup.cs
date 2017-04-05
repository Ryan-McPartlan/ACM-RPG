using UnityEngine;
using System.Collections;

public class FlightPickup : TileEffect {

    [SerializeField]
    GameObject flightEffect;

    public override void OnCharacterEnter(Unit unit)
    {
        unit.AddEffect(flightEffect);
        OnRemove();
    }
}
