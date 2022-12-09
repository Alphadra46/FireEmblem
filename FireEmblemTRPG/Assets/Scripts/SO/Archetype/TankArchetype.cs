using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArchetype : BaseArchetype
{
    protected override void InitStats()
    {
        maxHP = 30;
        hp = maxHP;
        movement = 4;
        strength = 12;
        magic = 2;
        dexterity = 5;
        speed = 7;
        luck = 5;
        defense = 8;
        resistance = 1;
        job = "Charcutier";
    }

    protected override void InitWeapon()
    {
        equippedWeapon = new WeaponStruct()
        {
            weaponName = "Ribz",
            might = 8,
            rangeMin = 1,
            rangeMax = 1,
            crit = 0,
            hit = 70,
            weigth = 7,
            weaponType = "Weapon"
        };
    }

    protected override void CalculateDerivedStats()
    {
        //attack = strength + equippedWeapon.might; //TODO - Move to Take damage
        attackSpeed = speed - equippedWeapon.weigth;
        hitRate = dexterity + equippedWeapon.hit;
        avoidanceRate = speed - equippedWeapon.weigth;
        criticalHitRate = (dexterity + luck) / 2 + equippedWeapon.crit;
        criticalAvoidanceRate = luck;
    }

    
}
