using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private BaseArchetype attacker;
    [SerializeField] private BaseArchetype defender;

    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        InputManagerScript.instance.action.started += Attack;//TODO - Remove this line and Start Attack when the user press the attack button in the UI menu
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Attack(InputAction.CallbackContext context)
    {
        StartAttack(attacker, defender);
    }
    

    private void StartAttack(BaseArchetype attacker, BaseArchetype defender)
    {
        //TODO - Vérifier le nombre d'action possible par les deux personnages (Riposte possible ou non ainsi que l'action double si la différence d'Attack Speed est de 4 ou plus) 
        
        if (Random.Range(1, 100) > attacker.hitRate - defender.avoidanceRate)
            return;
        
        Debug.Log(attacker.hitRate - defender.avoidanceRate);
        defender.TakeDamage(attacker);
        
        //TODO - Pour la riposte il faut vérifier que l'ennemi ne soit pas mort mais aussi vérifier s'il peut riposter (Ex : CaC vs Range, le Range ne peut pas riposter contre le CaC et inversemen. Cela est dû à la range de 2 du dps range)
    }

    
}
