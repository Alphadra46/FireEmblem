using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillClass 
{
    public string skillName;
    public string description;
    public int cooldown;
    public int turnLeftBeforeReUse;
    public int duration;
    public string targetLayer;
    public int durationLeft;

    public abstract void Init();

}
