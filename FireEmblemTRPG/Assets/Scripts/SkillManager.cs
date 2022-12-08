using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    
    public List<SkillClass> skillList = new List<SkillClass>();
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        skillList.Add(new Heal());
        skillList.Add(new DefenseBuff());
        skillList.Add(new LongRangeAttack());
        skillList.Add(new StunAttack());
        
        foreach (var skill in skillList)
        {
            skill.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
