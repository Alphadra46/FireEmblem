using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public GameObject attaqueUI;
    public GameObject contextUI;

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
        }
    }
    public void ToggleContextUI()
    {
        contextUI.SetActive(!contextUI.activeSelf); //set false if active and true if unactive

        if(contextUI.activeSelf)
        {
            Time.timeScale = 0;
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
        //add the wait function
    }
}