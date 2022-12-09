using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerArchetype : BaseArchetype
{
    protected override void InitStats()
    {
        maxHP = 22;
        hp = maxHP;
        movement = 4;
        strength = 6;
        magic = 12;
        dexterity = 6;
        speed = 7;
        luck = 6;
        defense = 4;
        resistance = 7;
        job = "Fromager";
    }

    protected override void InitWeapon()
    {
        equippedWeapon = new WeaponStruct()
        {
            weaponName = "Beau-Fort",
            might = 5,
            rangeMin = 1,
            rangeMax = 2,
            crit = 0,
            hit = 80,
            weigth = 5,
            weaponType = "Spell"
        };
    }
    
    protected override void CalculateDerivedStats()
    {
        attack = magic + equippedWeapon.might;//TODO - Move to Take damage
        attackSpeed = speed - equippedWeapon.weigth;
        hitRate = (dexterity+luck)/2 + equippedWeapon.hit;
        avoidanceRate = speed - equippedWeapon.weigth;
        criticalHitRate = (dexterity + luck) / 2 + equippedWeapon.crit;
        criticalAvoidanceRate = luck;
    }
}
