using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;
    
    public GameObject attaqueUI;
    public GameObject contextUI;
    public GameObject skillUI;
    public EventSystem eventSystem;
    [HideInInspector] public UIAttackValues uiAttackValues;
    [HideInInspector] public UISkillValues uiSkillValues;
    public CursorController cursorController;

    private void Awake()
    {
        instance = this;
        eventSystem = EventSystem.current;
        uiAttackValues = GetComponent<UIAttackValues>();
        uiSkillValues = GetComponent<UISkillValues>();
    }

    public void ToggleAttaqueUI()
    {
        attaqueUI.SetActive(!attaqueUI.activeSelf); //set false if active and true if unactive

        if(attaqueUI.activeSelf)
        {
            Time.timeScale = 0;
            eventSystem.SetSelectedGameObject(attaqueUI.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        }
        else
        {
            Time.timeScale = 1;
            InputManagerScript.instance.OnEnablePlayerControls();
        }

        
    }
    
    public void ToggleContextUI()
    {
        contextUI.SetActive(!contextUI.activeSelf); //set false if active and true if unactive

        if(contextUI.activeSelf)
        {
            Time.timeScale = 0;
            InputManagerScript.instance.OnDisablePlayerControls();
            
            if (contextUI.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                eventSystem.SetSelectedGameObject(contextUI.transform.GetChild(0).gameObject);
            }
            else
            {
                eventSystem.SetSelectedGameObject(contextUI.transform.GetChild(1).gameObject);
            }
        }
        else
        {
            Time.timeScale = 1;
        }

        
    }

    public void ToggleCapaciteUI()
    {
        skillUI.SetActive(!skillUI.activeSelf); //set false if active and true if unactive

        if(skillUI.activeSelf)
        {
            Time.timeScale = 0;
            eventSystem.SetSelectedGameObject(skillUI.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        }
        else
        {
            Time.timeScale = 1;
            InputManagerScript.instance.OnEnablePlayerControls();
        }
    }

    public void Wait() //May cause somme bugs
    {
        Debug.Log("Waiting...");
        InputManagerScript.instance.OnEnablePlayerControls();
        cursorController.selectedCharacterForAction.hasActionLeft = false;//TODO - Maybe change this
        cursorController.selectedCharacterForAction = null;
        //TODO also hide the attack tiles if the player Wait while he's in range for an attack
        //add the wait function
    }
}
