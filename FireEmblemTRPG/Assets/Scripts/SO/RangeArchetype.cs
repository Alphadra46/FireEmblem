using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeArchetype : BaseArchetype
{
    protected override void InitStats()
    {
        hp = 26;
        movement = 4;
        strength = 11;
        magic = 5;
        dexterity = 8;
        speed = 8;
        luck = 7;
        defense = 6;
        resistance = 4;
    }

    protected override void InitWeapon()
    {
        equippedWeapon = new WeaponStruct()
        {
            might = 6,
            rangeMin = 2,
            rangeMax = 2,
            crit = 0,
            hit = 85,
            weigth = 6
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
