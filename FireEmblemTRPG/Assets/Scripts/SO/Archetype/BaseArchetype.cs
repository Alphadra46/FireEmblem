using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseArchetype : MonoBehaviour
{
    //---------- Character Informations ----------
    public string characterName;
    [HideInInspector] public string job;
    
    //---------- Character Stats ----------
    [HideInInspector] public int maxHP;
    [HideInInspector] public int hp;
    [HideInInspector] public int movement;
    [HideInInspector] public int strength;
    [HideInInspector] public int magic;
    [HideInInspector] public int dexterity;
    [HideInInspector] public int speed;
    [HideInInspector] public int luck;
    [HideInInspector] public int defense;
    [HideInInspector] public int resistance;
    
    //---------- Character Derived Stats ----------
    [HideInInspector] public int attack;
    [HideInInspector] public int attackSpeed;
    [HideInInspector] public int criticalHitRate;
    [HideInInspector] public int hitRate;
    [HideInInspector] public int avoidanceRate;
    [HideInInspector] public int criticalAvoidanceRate;

    //---------- Equipped Weapon ----------
    public WeaponStruct equippedWeapon;

    //---------- Private Variables ----------
    private int damage;

    //---------- Action Variables ----------
    [HideInInspector] public bool hasActionLeft = true;
    [HideInInspector] public bool hasMovementLeft = true;
    [HideInInspector] public bool canCounter = true;
    [HideInInspector] public bool isStun = false;
    
    //---------- Inspector Variables ----------
    public LayerMask layerMask;
    
    //---------- Skills Variables ----------
    public List<SkillClass> equippedSkillList = new List<SkillClass>();
    public enum EquippedSkill
    {
        Fromagie,
        TourneDos,
        Rascaille,
        Bugne
    }

    public List<EquippedSkill> selectedSkillsList = new List<EquippedSkill>();


    // Start is called before the first frame update
    void Start()
    {
        InitStats();
        InitSkills();
        InitWeapon();
        CalculateDerivedStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitSkills()
    {
        SkillClass skillToAdd = null;
        foreach (var item in selectedSkillsList)
        {
            foreach (var registeredSkill in SkillManager.instance.skillList)
            {
                if (registeredSkill.skillName == item.ToString())
                {
                    skillToAdd = registeredSkill;
                }
            }
            equippedSkillList.Add(skillToAdd);
        }
    }
    
    public void TakeDamage(BaseArchetype enemy,float damageModifier)
    {
        enemy.attack = Mathf.RoundToInt((enemy.equippedWeapon.weaponType=="Weapon"?enemy.strength:enemy.magic) * damageModifier + enemy.equippedWeapon.might * AccointanceValue(enemy)); 

        damage = (enemy.attack - (enemy.equippedWeapon.weaponType=="Weapon"?defense:resistance)) * CriticalHitValue(enemy, this);
        Debug.Log("Damage done : "+damage);
        hp -= damage;
        //TODO - Check the death of the character
    }


    public int CriticalHitValue(BaseArchetype attacker, BaseArchetype defender)
    {
        return Mathf.RoundToInt((attacker.dexterity + attacker.luck)/2) + attacker.equippedWeapon.crit - defender.criticalAvoidanceRate > Random.Range(1, 100)? 3:1;
    }

    /// <summary>
    /// Determine the value of the Accointance system to know if the attacker have an advantage over the enemy or not
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public float AccointanceValue(BaseArchetype enemy)
    {
        float value = ((job == "Fromager" && enemy.job == "Poissoniere") || (job == "Charcutier" && enemy.job == "Fromager") || (job == "Boulangere" && enemy.job == "Charcutier") || (job == "Poissoniere" && enemy.job == "Boulangere")) ? 1.5f : 1;
        return value;
    }
    
    protected abstract void InitWeapon();
    protected abstract void InitStats();
    protected abstract void CalculateDerivedStats();

}
