using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{

    public string levelToLoad = "MainLevel";
    public GameObject mainMenu;
    public GameObject tutoMenu;
    public GameObject optionMenu;
    public GameObject creditsMenu;
    //public SceneFade sceneFade; add sceneFader
    public void Play()
    {
        SceneManager.LoadScene(levelToLoad); //change name for the mainScene
        //sceneFade.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ToggleOptionsMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionMenu.SetActive(!optionMenu.activeSelf);
    }
    public void ToggleCreditsMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        creditsMenu.SetActive(!creditsMenu.activeSelf);
    }

    public void StartTuto()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        tutoMenu.SetActive(!tutoMenu.activeSelf);
    }
}
