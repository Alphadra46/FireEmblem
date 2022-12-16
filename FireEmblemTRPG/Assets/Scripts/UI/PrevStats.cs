using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrevStats : MonoBehaviour
{
    //TODO Make the reference to these archetypes
    public BaseArchetype selectedCharacter;
    public BaseArchetype selectedEnemy;

    public GameObject prevUI;
    public GameObject attaqueUI;
    public Slider characterSlider;
    public Slider enemySlider;
    
    public Slider characterDamageTakenSlider;
    public Slider enemyDamageTakenSlider;


    private int damageCharacter;
    private int attackCharacter;

    private int damageEnemy;
    private int attackEnemy;

    [Header("Stats")]
    public TextMeshProUGUI atqStatValue;
    public TextMeshProUGUI atqStatValueEN;
    public TextMeshProUGUI prcStatValue;
    public TextMeshProUGUI prcStatValueEN;
    public TextMeshProUGUI critStatValue;
    public TextMeshProUGUI critStatValueEN;

    [Header("Pv Bar")]
    public TextMeshProUGUI pVActualCharacter;
    public TextMeshProUGUI pVActualEN;
    public TextMeshProUGUI pVMaxCharacter;
    public TextMeshProUGUI pVMaxEN;

    [Header("Enemy Infos")]
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyWeapon;

    [Header("Character Infos")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI archetypeName;
    public TextMeshProUGUI actualWeapon;

    public void OnSelection(float damageModifier) //TODO actualiser la fonction lors du passage sur un enemis et pas lors du choix de l'arme
    {
        PreviewAttackCharacter(damageModifier);
        PreviewAttackEnemy();
        ActualiseStats();
    }

    private void PreviewAttackCharacter(float damageModifier)
    {
        attackCharacter = Mathf.RoundToInt((selectedCharacter.equippedWeapon.weaponType=="Weapon"?selectedCharacter.strength:selectedCharacter.magic) * damageModifier + selectedCharacter.equippedWeapon.might * AccointanceValue(selectedEnemy, selectedCharacter));
        damageCharacter = (attackCharacter - (selectedCharacter.equippedWeapon.weaponType=="Weapon"?selectedEnemy.defense:selectedEnemy.resistance));
    }
    private void PreviewAttackEnemy()
    {
        attackEnemy = Mathf.RoundToInt((selectedEnemy.equippedWeapon.weaponType=="Weapon"?selectedEnemy.strength:selectedEnemy.magic) * 0.5f + selectedEnemy.equippedWeapon.might * AccointanceValue(selectedCharacter, selectedEnemy));
        damageEnemy = (attackEnemy - (selectedEnemy.equippedWeapon.weaponType=="Weapon"?selectedCharacter.defense:selectedCharacter.resistance));
    }

    public float AccointanceValue(BaseArchetype enemy, BaseArchetype character)
    {
        float value = ((character.job == "Fromager" && enemy.job == "Poissoniere") || (character.job == "Charcutier" && enemy.job == "Fromager") || (character.job == "Boulangere" && enemy.job == "Charcutier") || (character.job == "Poissoniere" && enemy.job == "Boulangere")) ? 1.5f : 1;
        return value;
    }

    private void ActualiseStats()
    {
        //selectedCharacter = cursorController.selectedCharacterForAction;

        //character stats
        atqStatValue.text = damageCharacter.ToString();
        prcStatValue.text = selectedCharacter.hitRate.ToString();
        critStatValue.text = selectedCharacter.criticalHitRate.ToString();

        //character Pv Bar
        pVActualCharacter.text = (Mathf.Clamp(selectedCharacter.hp - damageEnemy, 0, 1000)).ToString(); //TODO faire la modif en fonction de l'atk
        pVMaxCharacter.text = selectedCharacter.maxHP.ToString();

        //character infos
        characterName.text = selectedCharacter.characterName;
        //TODO archetypeName.text = selectedCharacter.archetypeName;
        actualWeapon.text = selectedCharacter.equippedWeapon.weaponName;



        //enemy stats
        atqStatValueEN.text = damageEnemy.ToString();
        prcStatValueEN.text = selectedEnemy.hitRate.ToString();
        critStatValueEN.text = selectedEnemy.criticalHitRate.ToString();

        //enemy Pv Bar
        pVActualEN.text = (Mathf.Clamp(selectedEnemy.hp - damageCharacter, 0, 1000)).ToString(); //TODO faire la modif en fonction de l'atk
        pVMaxEN.text = selectedEnemy.maxHP.ToString();

        //enemy infos
        enemyName.text = selectedEnemy.characterName;
        enemyWeapon.text = selectedEnemy.equippedWeapon.weaponName;

        characterSlider.value = (float)Mathf.Clamp(selectedCharacter.hp - damageEnemy, 0, 1000) / selectedCharacter.maxHP;
        enemySlider.value = Mathf.Clamp(selectedEnemy.hp - damageCharacter, 0, 1000) / selectedEnemy.maxHP;

        characterDamageTakenSlider.value = (float)selectedCharacter.hp / selectedCharacter.maxHP;
        enemyDamageTakenSlider.value = (float)selectedEnemy.hp / selectedEnemy.maxHP;
    }





    public void TogglePrevUI()
    {
        prevUI.SetActive(!prevUI.activeSelf); //set false if active and true if unactive

        if(prevUI.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            InputManagerScript.instance.OnEnablePlayerControls();
        }
    }

    public void ToggleAttaqueUI()
    {
        attaqueUI.SetActive(!attaqueUI.activeSelf); //set false if active and true if unactive

        if(attaqueUI.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            InputManagerScript.instance.OnEnablePlayerControls();
        }
        
    }


}
