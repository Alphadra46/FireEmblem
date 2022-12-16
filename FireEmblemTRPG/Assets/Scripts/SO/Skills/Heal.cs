using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SkillClass
{
    public override void Init()
    {
        skillName = "Fromagie";
        description = "Soin de 45% de la cible";
        cooldown = 2;
        turnLeftBeforeReUse = 0;
        duration = 0;
        targetLayer = "Ally";
    }

    public void Effect(BaseArchetype target)
    {
        target.hp += Mathf.Clamp(target.maxHP * 45 / 100, 0, target.maxHP - target.hp);
        turnLeftBeforeReUse = cooldown;
    }
}
