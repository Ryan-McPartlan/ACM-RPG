using UnityEngine;

public class TileEffect : MonoBehaviour {

    float nextTick;
    public float vanishTime;

    public Unit caster;
    public Tile affectedTile;

    // Use this for initialization
    protected virtual void Start () {

	}

    // Update is called once per frame
    protected virtual void Update() {

        //This implementation causes effects to "tick" when applied.
        if(Time.time > nextTick)
        {
            OnTick();
        }

	    if(Time.time > vanishTime)
        {
            OnFinish();
            OnRemove();
        }
	}

    public virtual void OnApply() {}

    public virtual void OnCharacterEnter(Unit unit) { }

    public virtual void OnCharacterExit(Unit unit) { }

    protected virtual void OnTick()
    {
        nextTick += 1;
    }

    public virtual void OnRemove()
    {
        affectedTile.effectsOnTile.Remove(this);
        Destroy(gameObject);
    }

    protected virtual void OnFinish() { }
}
