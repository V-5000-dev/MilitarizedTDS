using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDebug : MonoBehaviour
{
    void Start()
    {

        Debug.Log("EnableDebug");
    }
    
    public void DebugButton(string ButtonName)
    {
        Debug.Log("Button" + ButtonName + "Clicked.");
    }
}
