using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaCArchetype : BaseArchetype
{
    protected override void InitStats()
    {
        maxHP = 27;
        hp = maxHP;
        movement = 4;
        strength = 13;
        magic = 6;
        dexterity = 9;
        speed = 8;
        luck = 8;
        defense = 6;
        resistance = 6;
        job = "Poissoniere";
    }

    protected override void InitWeapon()
    {
        equippedWeapon = new WeaponStruct()
        {
            might = 5,
            rangeMin = 1,
            rangeMax = 1,
            crit = 0,
            hit = 90,
            weigth = 5,
            weaponType = "Weapon"
        };
    }
    
    protected override void CalculateDerivedStats()
    {
        attack = strength + equippedWeapon.might;//TODO - Move to Take damage
        attackSpeed = speed - equippedWeapon.weigth;
        hitRate = dexterity + equippedWeapon.hit;
        avoidanceRate = speed - equippedWeapon.weigth;
        criticalHitRate = (dexterity + luck) / 2 + equippedWeapon.crit;
        criticalAvoidanceRate = luck;
    }
}
