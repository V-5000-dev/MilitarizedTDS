using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public int gridID;
    public bool occupied;
    public Building connectedBuilding;
    public MeshRenderer meshRenderer;

    [SerializeField]
    public Color colorDefault;

    
    // Start is called before the first frame update
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        BuildBuilidng b = FindObjectOfType<BuildBuilidng>();


        for (int i = 0; i < b.grid.Length; i++)
        {
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
