using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
[System.Serializable]
public class Info : MonoBehaviour
{
    public Button btnDestory;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI discText;
    public Image[] levelImages;
    public Image towerlevel;
    private BuildBuilding build;
    private Building selectedBuilding;
    private TowerShoot selectedTower;
    
   
    // Start is called before the first frame update
    void Awake()
    {
       
        build = FindObjectOfType<BuildBuilding>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((build.currentSelectedGridElement != null && build.currentSelectedGridElement.connectedBuilding != null) || build.currentSelectedBuilding != null)
        {         
        //    selectedBuilding = build.currentSelectedGridElement.connectedBuilding;
         //   nameText.text = selectedBuilding.objName;
         //   discText.text = selectedBuilding.objDisc;
            DisplayInfo();
        }
        else
        {
            
            nameText.text = "No Building Selected.";
            selectedBuilding = null;
            discText.text = " ";
            towerlevel = null;
           
          
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
        
    }
    public void DisplayInfo()
    {

        selectedBuilding = build.currentSelectedGridElement.connectedBuilding;
        selectedTower = selectedBuilding.gameObject.GetComponent<TowerShoot>();
        if (selectedBuilding == null)
            selectedBuilding = build.currentSelectedBuilding.GetComponent<Building>();
        nameText.text = selectedBuilding.objName;
        discText.text = $"{selectedBuilding.objDisc} \n Damage: {selectedTower.damage} \n Armor Pen: {selectedTower.armorPen} \n Fire Rate: {selectedTower.fireRate} \n Range: {selectedTower.range} \n Mag Size: {selectedTower.magSize} \n Reload Speed {selectedTower.reloadSpeed} \n Hidden Detection: {selectedTower.hiddenDetect}"
        ;
        
    }
}
