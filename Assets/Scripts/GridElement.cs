using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public int gridID;
    public bool occupied;
    public Building connectedBuilding;
    public MeshRenderer meshRenderer;


    
    // Start is called before the first frame update
    private void Start()
    {


        for (int i = 0; i < b.grid.Length; i++)
        {
            Debug.Log(b.grid[i].transform == transform);
            if (b.grid[i].transform == transform)
            {
                gridID = i;
                
                break;
            }
        }
        return;
        
     
    }
    private void Update()
    {


    }


}
