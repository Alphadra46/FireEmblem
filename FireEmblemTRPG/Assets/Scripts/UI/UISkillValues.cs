using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkillValues : MonoBehaviour
{
    
    [HideInInspector] public BaseArchetype selectedCharacter;
    
    [Header("Skill")]
    public TextMeshProUGUI actualSkill;

    [Header("Stats")]
    public TextMeshProUGUI cooldownValue;
    public TextMeshProUGUI durationValue;
    public TextMeshProUGUI skillDescriptionValue;
    public TextMeshProUGUI turnLeftBeforeReUseValue;
    public TextMeshProUGUI atkValue;
    public TextMeshProUGUI hitValue;
    public TextMeshProUGUI critValue;
    public TextMeshProUGUI asValue;
    public TextMeshProUGUI defValue;
    public TextMeshProUGUI resValue;
    public TextMeshProUGUI avoValue;
    public TextMeshProUGUI rngValue;

    [Header("Character")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI lvlValue;
    public TextMeshProUGUI hpValue;
    public TextMeshProUGUI classeValue;


    [Header("Layer")] 
    [SerializeField] private LayerMask allyLayer;
    [SerializeField] private LayerMask enemyLayer;
    
    public void OnSelection()
    {
        Debug.Log(selectedCharacter.equippedSkillList[0]);
        hitValue.text = selectedCharacter.hitRate.ToString();
        critValue.text = selectedCharacter.criticalHitRate.ToString();
        asValue.text = selectedCharacter.attackSpeed.ToString();
        defValue.text = selectedCharacter.defense.ToString();
        resValue.text = selectedCharacter.resistance.ToString();
        avoValue.text = selectedCharacter.avoidanceRate.ToString();

        hpValue.text = selectedCharacter.hp + "/" + selectedCharacter.maxHP;
        
        if (selectedCharacter.equippedWeapon.rangeMin == selectedCharacter.equippedWeapon.rangeMax)
        {
            rngValue.text = selectedCharacter.equippedWeapon.rangeMax.ToString();
        }
        else
        {
            rngValue.text = selectedCharacter.equippedWeapon.rangeMin + "-" + selectedCharacter.equippedWeapon.rangeMax;
        }
        
        actualSkill.text = selectedCharacter.equippedSkillList[0].skillName;

        characterName.text = selectedCharacter.characterName;

        cooldownValue.text = selectedCharacter.equippedSkillList[0].cooldown.ToString();
        durationValue.text = selectedCharacter.equippedSkillList[0].duration.ToString();
        skillDescriptionValue.text = selectedCharacter.equippedSkillList[0].description;
        turnLeftBeforeReUseValue.text = selectedCharacter.equippedSkillList[0].turnLeftBeforeReUse.ToString();
    }

    public void OnSkillChoosed()
    {
        if (selectedCharacter.equippedSkillList[0].targetLayer == "Enemy")
        {
            ContextMenu.instance.cursorController.isAttacking = true;
            ContextMenu.instance.cursorController.isUsingSkill = true;
            
        }
        else
        {
            if (selectedCharacter.equippedSkillList[0].skillName == "TourneDos")
            {
                DefenseBuff defBuff = (DefenseBuff)selectedCharacter.equippedSkillList[0];
                defBuff.Effect(selectedCharacter);
                ContextMenu.instance.cursorController.isUsingSkill = true;
            }
            else if (selectedCharacter.equippedSkillList[0].skillName == "Fromagie")
            {
                ContextMenu.instance.cursorController.GetInRangeHealTiles(ContextMenu.instance.cursorController.CharacterCurrentStandingTile());
                foreach (var item in ContextMenu.instance.cursorController.inRangeAttackPhaseTiles)
                {
                    item.ShowTile();
                    item.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
            else
            {
                ContextMenu.instance.Wait();//Change to something else
            }
        }
    }
}
