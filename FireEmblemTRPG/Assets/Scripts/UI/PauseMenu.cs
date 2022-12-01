using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    //public SceneFade sceneFade;
    public string menuSceneName = "MainMenu";

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
        */
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
        Toggle();
        SceneManager.LoadScene(menuSceneName);
        //sceneFade.FadeTo(menuSceneName);
    }
}
