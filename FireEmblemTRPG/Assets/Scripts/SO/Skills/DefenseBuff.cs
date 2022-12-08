using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : SkillClass
{
    private int initialResistance;
    public override void Init()
    {
        skillName = "Tourne-dos";
        duration = 2;
        description = "La RES prend la valeur de DEF pendant "+duration+" tours.";
        turnLeftBeforeReUse = 0;
        cooldown = 3;
    }

    public void Effect(BaseArchetype target)
    {
        initialResistance = target.resistance;
        target.resistance = target.defense;
    }

    public void DisableEffect(BaseArchetype target)
    {
        target.resistance = initialResistance;
    }
}
