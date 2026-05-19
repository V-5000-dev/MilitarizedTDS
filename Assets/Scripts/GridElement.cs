using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public int gridID;
    public bool occupied;
    public int gridType;
    public Building connectedBuilding;
    public MeshRenderer meshRenderer;

    public Color colorDefault;

    
    // Start is called before the first frame update
    private void Start()
    {
        gridType = Random.Range(1, 10);
        meshRenderer = GetComponent<MeshRenderer>();
        BuildBuilidng b = FindObjectOfType<BuildBuilidng>();


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
 
                meshRenderer.material.color = colorDefault;
                gameObject.tag = "Grid";
    


        
       
        

    }


}
