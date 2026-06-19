 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Building : MonoBehaviour
{
    public BuildingInfo info;
    public float price;
    public string objName;
    public string objDisc;

    public bool placed;


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
