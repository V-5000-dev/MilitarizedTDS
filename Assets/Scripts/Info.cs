using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class Info : MonoBehaviour
{
    public Button btnDestory;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI discText;
    public TextMeshProUGUI incomeText;
    public TextMeshProUGUI expenseText;
    private BuildBuilidng build;
    private Building selectedBuilding;
    private Resources resources;
    
   
    // Start is called before the first frame update
    void Awake()
    {
        build = FindObjectOfType<BuildBuilidng>();
        resources = FindObjectOfType<Resources>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((build.currentSelectedGridElement != null && build.currentSelectedGridElement.connectedBuilding != null) || build.currentSelectedBuilding != null)
        {
            /*
            selectedBuilding = build.currentSelectedGridElement.connectedBuilding;
            nameText.text = selectedBuilding.objName;
            discText.text = selectedBuilding.objDisc;
            incomeText.text = selectedBuilding.objIncome;
            expenseText.text = selectedBuilding.objExpense;
            */
            DisplayInfo();
        }
        else
        {
            
            nameText.text = "No Building Selected.";
            selectedBuilding = null;
            discText.text = " ";
            incomeText.text = " ";
            expenseText.text = " ";
            
           
          
        }
        if (selectedBuilding)
            btnDestory.interactable = selectedBuilding;
        else
            btnDestory.interactable = false;
    
    }
    public void OnBtnDestory()
    {
        if (selectedBuilding)
            build.currentSelectedGridElement.occupied = false;
        RefundResources();
        build.buildings.builtObjects.Remove(selectedBuilding.gameObject);
        Destroy(selectedBuilding.gameObject);
        
    }
    public void RefundResources()
    {
        resources.wood += selectedBuilding.price.price_wood;
        resources.stone += selectedBuilding.price.price_stone;
        
    }
    public void DisplayInfo()
    {
        
        selectedBuilding = build.currentSelectedGridElement.connectedBuilding;
        if(selectedBuilding == null)
            selectedBuilding = build.currentSelectedBuilding.GetComponent<Building>();
        nameText.text = selectedBuilding.objName;
        discText.text = selectedBuilding.objDisc;
        incomeText.text = selectedBuilding.objIncome;
        expenseText.text = selectedBuilding.objExpense;
    }
}
