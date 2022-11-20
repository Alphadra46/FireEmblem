using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAttackValues : MonoBehaviour
{
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

}
