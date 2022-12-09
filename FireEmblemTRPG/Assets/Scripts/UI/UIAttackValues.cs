using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAttackValues : MonoBehaviour
{
    [HideInInspector] public BaseArchetype selectedCharacter;
    private BaseArchetype selectedEnemy;
    
    [SerializeField] private CursorController cursorController;

    [Header("Weapons")]
    public TextMeshProUGUI actualWeapon;

    [Header("Stats")]
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

    public void OnSelection()
    {
        //selectedCharacter = cursorController.selectedCharacterForAction;
        //atkValue.text = selectedCharacter.attack.ToString();
        
        hitValue.text = selectedCharacter.hitRate.ToString();
        critValue.text = selectedCharacter.criticalHitRate.ToString();
        asValue.text = selectedCharacter.attackSpeed.ToString();
        defValue.text = selectedCharacter.defense.ToString();
        resValue.text = selectedCharacter.resistance.ToString();
        avoValue.text = selectedCharacter.avoidanceRate.ToString();

        hpValue.text = selectedCharacter.hp.ToString() + "/" + selectedCharacter.maxHP.ToString();//change for maxHP

        if (selectedCharacter.equippedWeapon.rangeMin == selectedCharacter.equippedWeapon.rangeMax)
        {
            rngValue.text = selectedCharacter.equippedWeapon.rangeMax.ToString();
        }
        else
        {
            rngValue.text = selectedCharacter.equippedWeapon.rangeMin.ToString() + "-" + selectedCharacter.equippedWeapon.rangeMax.ToString();
        }

        actualWeapon.text = selectedCharacter.equippedWeapon.weaponName;

        characterName.text = selectedCharacter.characterName;
        //lvlValue.text = selectedCharacter.attack.ToString();
        //classeValue.text = selectedCharacter.attack.ToString();
    }

    public void OnWeaponChoosed()
    {
        cursorController.isAttacking = true;
    }

}
