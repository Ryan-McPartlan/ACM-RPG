using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Vector2 matrixPosition;
    public List<Unit> unitsOnTile = new List<Unit>(); //TODO: Handle after implementing FightingUnit
    //List<item> itemsOnTile; TODO: Handle after implementing item
    public List<TileEffect> effectsOnTile = new List<TileEffect>();
    public bool blocksMovement;
    public bool blocksFliers;
    public bool blocksVision;
    public bool blocksProjectiles;

    public void Setup(Vector2 pos, bool move)
    {
        matrixPosition = pos;
        blocksMovement = move;
    }

    public void EnterTile(Unit unit)
    {
        unitsOnTile.Add(unit);

        for(int i = 0; i < effectsOnTile.Count; i++)
        {
            effectsOnTile[i].OnCharacterEnter(unit);
        }
    }
    
    public void LeaveTile(Unit unit){
        unitsOnTile.Remove(unit);
        for (int i = 0; i < effectsOnTile.Count; i++)
        {
            effectsOnTile[i].OnCharacterExit(unit);
        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!GameController.gameController.paused)
            {
                PlayerController.playerController.TileClicked(this);
            }
        }
    }

    //An ability, projectile, or something else with a referance to a tileEffect prefab
    public void AddEffect(GameObject effect, float duration = 30, Unit caster = null)
    {
        GameObject newEffectObject = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
        newEffectObject.transform.parent = transform;

        TileEffect newEffect = newEffectObject.GetComponent<TileEffect>();

        effectsOnTile.Add(newEffect);
        newEffect.affectedTile = this;
        newEffect.vanishTime = Time.time + duration;
        newEffect.OnApply();
    }

    /*
    AddItem(){
    }
    RemoveItem(){
    }
    KickOut(){
    }
    */
}
