using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;
    
    public GameObject attaqueUI;
    public GameObject contextUI;
    [HideInInspector] public UIAttackValues uiAttackValues;

    

    private void Awake()
    {
        instance = this;
        uiAttackValues = GetComponent<UIAttackValues>();
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
    public void ToggleContextUI()
    {
        contextUI.SetActive(!contextUI.activeSelf); //set false if active and true if unactive

        if(contextUI.activeSelf)
        {
            Time.timeScale = 0;
            InputManagerScript.instance.OnDisablePlayerControls();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ToggleCapaciteUI()
    {
        Debug.Log("Open capacity menu");
        //add the capacity menu
    }

    public void Wait()
    {
        Debug.Log("Waiting...");
        InputManagerScript.instance.OnEnablePlayerControls();
        //TODO also hide the attack tiles if the player Wait while he's in range for an attack
        //add the wait function
    }
}
