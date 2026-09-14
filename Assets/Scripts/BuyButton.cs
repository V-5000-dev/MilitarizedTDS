using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BuyButton : MonoBehaviour
{
    public int connectedBuildingID;

    [HideInInspector]
    public Building connectedBuilding;
    public TextMeshProUGUI text;
    public Color defaultColor;
    private BuildBuilding buildBuilding;
    public int id;
    public int cost;


    private Button btn;

    public bool isInteractable = false;


    // Start is called before the first frame update
    void Awake()
    {
        buildBuilding = FindFirstObjectByType<BuildBuilding>();
        btn = GetComponent<Button>();
        Buildings buildings = FindObjectOfType<Buildings>();
        if (buildings == null) return;

        foreach (GameObject g0 in buildings.buildabables)
        {
            BuildingInfo info = g0.GetComponent<BuildingInfo>();
            if (info != null && info.ID == connectedBuildingID)
            {
                connectedBuilding = g0.GetComponent<Building>();
                break;
            }

        }


        // Update is called once per frame
        
    }
    void Start()
    {
        btn.onClick.AddListener(() => 
        {
            if(MoneyController.money < cost)
            {
                StartCoroutine(MoneyController.NotEnoughMoney());
                return;
            }
        
            else
            {
                buildBuilding.OnButtonCreateBuilding(connectedBuildingID);
                MoneyController.RemoveMoney(cost);
            }
        });
    }   
}
