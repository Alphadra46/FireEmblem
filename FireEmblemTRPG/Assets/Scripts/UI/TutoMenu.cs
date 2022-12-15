using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoMenu : MonoBehaviour
{
    public GameObject tutoMenu;
    public GameObject gameManager;
    public int tutoIndex = 0;

    public void NextTuto()
    {
        if (tutoIndex < tutoMenu.transform.childCount - 1)
        {
            tutoMenu.transform.GetChild(tutoIndex).gameObject.SetActive(false);
            tutoMenu.transform.GetChild(tutoIndex + 1).gameObject.SetActive(true);

            tutoIndex++;
        }
        else
        {
            gameManager.GetComponent<UiMainMenu>().Play();
        }
        
    }

    public void PreviousTuto()
    {
        if (tutoIndex > 0)
        {
            tutoMenu.transform.GetChild(tutoIndex).gameObject.SetActive(false);
            tutoMenu.transform.GetChild(tutoIndex - 1).gameObject.SetActive(true);

            tutoIndex--;
        }
        else
        {
            gameManager.GetComponent<UiMainMenu>().StartTuto();
        }
        
    }
}
