 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Building : MonoBehaviour
{
    public BuildingInfo info;
    public BuildingPrice price;
    public string objName;
    public string objDisc;
    public string objIncome;
    public string objExpense;
    public bool placed;
    public string requiredTag;

    public string requiredBuilding;
    public int requiredDistanceFromBuidling;

    private Resources resources;

    public float requiredDistanceFromBuilding { get; internal set; }

    // Start is called before the first frame update
    void Awake()
    {
        resources = FindObjectOfType<Resources>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!placed)
        {
            return;
        }
        switch (info.ID)
        {
            //Lumberjack
            case 1:
                resources.wood += (info.buildingRecourceProduction * info.buildingLevel) * Time.deltaTime;
                
                return;
                //Stone Mason
            case 2:
                resources.stone += (info.buildingRecourceProduction * info.buildingLevel) * Time.deltaTime;
                return;
                // Farm
            case 3:
                resources.food += (info.buildingRecourceProduction * info.buildingLevel) * Time.deltaTime;

                return;
            case 4:
                // House
                resources.food -= info.buildingRecourceProduction * Time.deltaTime;
                resources.workers += info.baseBuildingProduction;
                return;
            case 5:
                // Sawmill
                resources.planks += (info.buildingRecourceProduction * info.buildingLevel) * Time.deltaTime*2;
                resources.wood -= (info.baseBuildingExpense * info.buildingLevel) * Time.deltaTime;
                return;



        }
       
    }
    public void ConstructBuilding()
    {
        resources.wood -= price.price_wood;
        resources.stone -= price.price_stone;
        resources.food -= price.price_food;
        resources.workers -= price.price_workers;
    }
}
