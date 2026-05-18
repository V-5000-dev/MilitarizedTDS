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

    public Color colorForest = Color.green;
    public Color colorFarmland = Color.yellow;
    public Color colorClay;
    public Color colorDefault;

    
    // Start is called before the first frame update
    private void Start()
    {
        gridType = Random.Range(1, 10);
        meshRenderer = GetComponent<MeshRenderer>();
        BuildBuilidng b = FindObjectOfType<BuildBuilidng>();

        GameObject trees = transform.Find("Trees").gameObject;
        GameObject rocks = transform.Find("Rocks").gameObject;


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
        GameObject trees = transform.Find("Trees").gameObject;
        GameObject rocks = transform.Find("Rocks").gameObject;




        switch (gridType)
        {
            case 1:
                if (occupied)
                    trees.SetActive(false);
                else
                    trees.SetActive(true);

                meshRenderer.material.color = colorForest;
                colorDefault = colorForest;
                gameObject.tag = "Grid Forest";
                return;
            case 2:
                meshRenderer.material.color = colorFarmland;
                colorDefault = colorFarmland;
                gameObject.tag = "Grid Farmland";
                return;
            case 3:
                if (occupied)
                    rocks.SetActive(false);
                else
                    rocks.SetActive(true);

                meshRenderer.material.color = colorForest;
                colorDefault = colorForest;
                gameObject.tag = "Grid Rocks";
                return;
            case 4:
                meshRenderer.material.color = colorClay;
                colorDefault = colorClay;
                gameObject.tag = "Grid Clay";
                return;


            default:
                meshRenderer.material.color = colorDefault;
                gameObject.tag = "Grid";
                return;


        }
       
        

    }


}
