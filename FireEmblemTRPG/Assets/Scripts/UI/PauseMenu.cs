using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    //public SceneFade sceneFade;
    public string menuSceneName = "MainMenu";
    private DefaultInputActions uiInputAction;
    private InputAction cancel;

    private void Awake()
    {
        Debug.Log("Awake");
        uiInputAction = new DefaultInputActions();
        cancel = uiInputAction.UI.Cancel;
        cancel.Enable();
        cancel.started += ActivateToggle;
    }

    private void ActivateToggle(InputAction.CallbackContext context)
    {
        Debug.Log("ok.");
        Toggle();
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf); //set false if active and true if unactive

        if (ui.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Retry()
    {
        Toggle();
        //sceneFade.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        cancel.Disable();
        SceneManager.LoadScene(menuSceneName);
        //sceneFade.FadeTo(menuSceneName);
    }
}
