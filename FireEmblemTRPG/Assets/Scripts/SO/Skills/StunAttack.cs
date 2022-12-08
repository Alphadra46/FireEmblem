using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAttack : SkillClass
{
    public override void Init()
    {
        skillName = "Rascaille";
        description = "Fait passer le tour de la cible";
        cooldown = 3;
        turnLeftBeforeReUse = 0;
        duration = 1;
    }

    public void Effect(BaseArchetype target)
    {
        target.isStun = true;
    }
}
