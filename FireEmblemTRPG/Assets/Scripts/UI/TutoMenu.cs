using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoMenu : MonoBehaviour
{
    public GameObject tutoMenu;
    public int tutoIndex = 0;

    public void NextTuto()
    {
        if (tutoMenu.transform.childCount > tutoIndex)
        {
            tutoMenu.transform.GetChild(tutoIndex).gameObject.SetActive(false);
            tutoMenu.transform.GetChild(tutoIndex + 1).gameObject.SetActive(true);

            tutoIndex++;
            //find a way to not have an error
        }
        
    }
}
