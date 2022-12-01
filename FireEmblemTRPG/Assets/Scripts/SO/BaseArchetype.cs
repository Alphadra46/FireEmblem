using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseArchetype : MonoBehaviour
{
    //---------- Character Stats ----------
    [HideInInspector] public int maxHP;
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
    
    //---------- Action Variables ----------
    [HideInInspector] public bool hasActionLeft = true;
    [HideInInspector] public bool hasMovementLeft = true;
    [HideInInspector] public bool canCounter = true;
    
    //---------- Inspector Variables ----------
    public LayerMask layerMask;
    
    
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
        //TODO - Add "Accointance" System
        enemy.attack = enemy.equippedWeapon.weaponType=="Weapon"?enemy.strength:enemy.magic + enemy.equippedWeapon.might; 

        damage = (enemy.attack - (enemy.equippedWeapon.weaponType=="Weapon"?defense:resistance)) * CriticalHitValue(enemy, this);
        Debug.Log("Damage done : "+damage);
        hp -= damage;
        //TODO - Check the death of the character
        //TODO - Counter attack of the defender if he's not dead and if he's in range
    }


    private int CriticalHitValue(BaseArchetype attacker, BaseArchetype defender)
    {
        return Mathf.RoundToInt((attacker.dexterity + attacker.luck)/ 2) + attacker.equippedWeapon.crit - defender.criticalAvoidanceRate > Random.Range(1, 100)? 3:1;
    }


    protected abstract void InitWeapon();
    protected abstract void InitStats();
    protected abstract void CalculateDerivedStats();

}
