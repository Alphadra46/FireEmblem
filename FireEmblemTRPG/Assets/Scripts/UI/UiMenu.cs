using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{

    public string levelToLoad = "MainLevel";
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

    private void Options()
    {

    }
    private void Credits()
    {

    }
}