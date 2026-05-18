using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableButton : MonoBehaviour
{
    public GameObject bottomBar;
    public GameObject[] otherBottomBars;
    public void EnableButton()
    {
        bottomBar.SetActive(!bottomBar.activeSelf);
        foreach (GameObject obj in otherBottomBars)
        {
            obj.SetActive(false);
        }
        
    }    
}
