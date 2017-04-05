using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public Vector3 origin;
    public float originTime;
    public float range;
    public float duration;
    public float speed;
    public Unit caster;
    public int damage;
    List<CharacterEffect> onHitEffects;

    protected virtual void Start()
    {
        origin = transform.position;
        originTime = Time.time;
    }

    //Check if were out of range or duration, then destroy this
    protected virtual void Update()
    {
        if(Vector3.Distance(transform.position, origin) > range)
        {
            Destroy(gameObject);
        }
        if (Time.time > duration + originTime && duration > 0)
        {
            Destroy(gameObject);
        }
        MovementPattern();
    }

    //Our movement pattern with each frame
    protected virtual void MovementPattern()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speed * Time.deltaTime);
    }
    
    //Called when we hit an ally, ourselves, an enemy, another projectile, or a wall
    protected virtual void OnHitAlly(Unit allyHit)
    {

    }
    protected virtual void OnHitEnemy(Unit enemyHit)
    {
        enemyHit.TakeDamage(damage);

        for(int i = 0; i < 50; i++)
        {

        }

        Destroy(gameObject);
    }
    protected virtual void OnHitSelf(Unit selfHit)
    {
        selfHit.TakeDamage(damage);
        Destroy(gameObject);
    }
    protected virtual void OnHitWall(Tile wallHit)
    {
        Destroy(gameObject);
    }
    protected virtual void OnHitProjectile(Projectile projectileHit)
    {
        Destroy(gameObject);
    }

    //This should probably not be ovveridden. Calls the various onhit function
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Unit")
        {
            Unit unitHit = other.GetComponent<Unit>();

            if (caster != null)
            {
                if(caster == unitHit)
                {
                    OnHitSelf(unitHit);
                }
                else if (other.GetComponent<Unit>().faction == caster.faction)
                {
                    OnHitAlly(unitHit);
                }
                else
                {
                    OnHitEnemy(unitHit);
                }
            }
            else
            {
                OnHitEnemy(unitHit);
            }
        }
        else if(other.tag == "Tile")
        {
            Tile tileHit = other.GetComponent<Tile>();

            if (other.GetComponent<Tile>().blocksProjectiles)
            {
                OnHitWall(tileHit);
            }
        }
        else if(other.tag == "Projectile")
        {
            Projectile projectileHit = other.GetComponent<Projectile>();

            OnHitProjectile(projectileHit);
        }
    }

}
