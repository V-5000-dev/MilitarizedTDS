using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyButton : MonoBehaviour
{
    public int connectedBuildingID;

    [HideInInspector]
    public Building connectedBuilding;
    public TextMeshProUGUI text;
    public Color defaultColor;

    private Button btn;
    private Resources resources;

    public bool isInteractable = false;


    // Start is called before the first frame update
    void Awake()
    {
        btn = GetComponent<Button>();
        resources = FindObjectOfType<Resources>();
        Buildings buildings = FindObjectOfType<Buildings>();

        foreach (GameObject g0 in buildings.buildabables)
        {
            Building b = g0.GetComponent<Building>();
            if (b.info.ID == connectedBuildingID)
            {
                connectedBuilding = b;
                break;
            }

        }


        // Update is called once per frame
        
    }
    public void Update()
    {

        if(resources.wood >= connectedBuilding.price.price_wood && resources.stone >= connectedBuilding.price.price_stone)
        {
            isInteractable = true;

        }
        btn.interactable = isInteractable;
        




    }

        

}
