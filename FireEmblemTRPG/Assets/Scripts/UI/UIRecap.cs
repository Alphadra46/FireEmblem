using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRecap : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI classe;
    public TextMeshProUGUI pv;

    public BaseArchetype selectedCharacter;

    public void OnSelection()
    {
        characterName.text = selectedCharacter.characterName;
        //TODO classe.text = selectedCharacter.archetypeName;
        pv.text = "PV : " + (selectedCharacter.hp + "/" + selectedCharacter.maxHP).ToString();
    }
}
