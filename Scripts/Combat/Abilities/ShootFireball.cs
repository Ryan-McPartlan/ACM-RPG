using UnityEngine;
using System.Collections;

public class ShootFireball : Ability {

    [SerializeField]
    GameObject fireball;

    [SerializeField]
    float instantiationDistance;

    protected override void UseAbility(Unit caster, Tile target)
    {
        Vector2 instatiationDirection = (target.transform.position - caster.transform.position).normalized;
        float instatiationRotation = Functions.VectorToAngle(instatiationDirection);

        Projectile newFireBall = (Instantiate(fireball, (Vector2)caster.transform.position + instatiationDirection * instantiationDistance, Quaternion.Euler(0, 0, instatiationRotation)) as GameObject).GetComponent<Projectile>();
        newFireBall.caster = caster;
        newFireBall.damage = 5;
    }
}
