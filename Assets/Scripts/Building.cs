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

    // Update is called once per frame
    void Update()
    {
        if(!placed)
        {
            return;
        }

       
    }
    public void ConstructBuilding()
    {

    }
}
