using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttack : SkillClass
{
    private int initialAttack;
    public override void Init()
    {
        skillName = "Bugne";
        description = "Augmente l'attaque de 25% puis inflige des dégâts à la cible";
        cooldown = 2;
        turnLeftBeforeReUse = 0;
        duration = 0;
        targetLayer = "Enemy";
    }

    public void Effect(BaseArchetype self, BaseArchetype target)
    {
        CombatManager.instance.StartAttack(self,target,1.25f);
        turnLeftBeforeReUse = cooldown;
    }
}
