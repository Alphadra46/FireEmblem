using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseArchetype : MonoBehaviour
{
    //---------- Character Stats ----------
    [HideInInspector] public int hp;
    [HideInInspector] public int movement;
    [HideInInspector] public int strength;
    [HideInInspector] public int magic;
    [HideInInspector] public int dexterity;
    [HideInInspector] public int speed;
    [HideInInspector] public int luck;
    [HideInInspector] public int defense;
    [HideInInspector] public int resistance;
    
    //---------- Character Derived Stats ----------
    [HideInInspector] public int attack;
    [HideInInspector] public int attackSpeed;
    [HideInInspector] public int criticalHitRate;
    [HideInInspector] public int hitRate;
    [HideInInspector] public int avoidanceRate;
    [HideInInspector] public int criticalAvoidanceRate;

    //---------- Equipped Weapon ----------
    public WeaponStruct equippedWeapon;

    //---------- Private Variables ----------
    private int damage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        InitStats();
        InitWeapon();
        CalculateDerivedStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(BaseArchetype enemy)
    {
        //TODO - Add Accointance System
        enemy.attack = enemy.strength + enemy.equippedWeapon.might; //TODO - Faire en sorte que cela fonctionne aussi pour les mages

        damage = (enemy.attack - defense) * CriticalHitValue(enemy, this);
        //TODO - Subtract to HP the amount of damage
    }


    private int CriticalHitValue(BaseArchetype attacker, BaseArchetype defender)
    {
        int critValue;
        return critValue = Mathf.RoundToInt((attacker.dexterity + attacker.luck)/ 2) + attacker.equippedWeapon.crit - defender.criticalAvoidanceRate > Random.Range(1, 100)? 3:1;
    }
    
    protected abstract void InitWeapon();
    protected abstract void InitStats();
    protected abstract void CalculateDerivedStats();

}
