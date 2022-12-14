using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour //TODO - Maybe make this class a singleton
{
    //[SerializeField] private BaseArchetype attacker;
    //[SerializeField] private BaseArchetype defender;

    public static CombatManager instance;
    private RangeFinder rangeFinder;
    [SerializeField] private CursorController cursorController;
    
    private int damage;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        rangeFinder = new RangeFinder();
    }
    
    
    public void StartAttack(BaseArchetype attacker, BaseArchetype defender, float damageModifier)
    {
        //TODO - Vérifier le nombre d'action possible par les deux personnages (Riposte possible ou non ainsi que l'action double si la différence d'Attack Speed est de 4 ou plus) 
        
        if (Random.Range(1, 100) > attacker.hitRate - defender.avoidanceRate) //TODO - Create a feedback for this
            return;
        
        defender.TakeDamage(attacker, damageModifier);

        if (defender.hp > 0 && defender.canCounter && IsCharacterInRangeForCounter(defender, attacker) && !defender.isStun)
        {
            StartAttack(defender,attacker, 0.5f);
            InputManagerScript.instance.OnEnablePlayerControls();
        }
        //ContextMenu.instance.Wait();//TODO - Maybe change this
    }

    private bool IsCharacterInRangeForCounter(BaseArchetype defender, BaseArchetype attacker)
    {
        bool canCounter = false;

        int defenderMaxRange = defender.equippedWeapon.rangeMax;
        int defenderMinRange = defender.equippedWeapon.rangeMin;
        
        var tilesInRange = rangeFinder.GetTilesInRange(cursorController.CharacterCurrentStandingTile(defender),defenderMaxRange );
        var tilesToRemove = rangeFinder.GetTilesInRange(cursorController.CharacterCurrentStandingTile(defender),  defenderMinRange- 1);

        if (defenderMaxRange == defenderMinRange)
        {
            foreach (var item in tilesToRemove)
            {
                tilesInRange.Remove(item);
            }
        }
        
        
        foreach (var item in tilesInRange)
        {
            Collider[] enemy = new Collider[10];
            Physics.OverlapBoxNonAlloc(item.transform.position,new Vector3(0.5f,0.5f,0.5f),enemy,new Quaternion(0,0,0,0), attacker.layerMask);
            foreach (var collider in enemy)
            {
                if (collider == null)
                    break;
                
                if (collider.gameObject == attacker.gameObject)
                {
                    canCounter = true;
                }
            }
            
        }
        return canCounter;
    }
}
