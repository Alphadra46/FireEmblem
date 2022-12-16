using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : SkillClass
{
    private int initialResistance;
    public override void Init()
    {
        skillName = "TourneDos";
        duration = 2;
        description = "La RES prend la valeur de DEF pendant "+duration+" tours.";
        turnLeftBeforeReUse = 0;
        cooldown = 3;
        targetLayer = "Ally";
    }

    public void Effect(BaseArchetype target)
    {
        initialResistance = target.resistance;
        target.resistance = target.defense;
        turnLeftBeforeReUse = cooldown;
        durationLeft = duration;
    }

    public void DisableEffect(BaseArchetype target)
    {
        target.resistance = initialResistance;
    }
}
