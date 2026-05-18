using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuildBuilidng : MonoBehaviour
{
    public GridElement currentSelectedGridElement;
    public GridElement currentHoveredGridElement;

    public Info info;

    public GridElement[] grid;

    public Buildings buildings;
    public BuyButton buyButton;

    [Header("Colors")]
    public Color colorHover = Color.white;
    public Color colorOnOccupied = Color.red;

    private RaycastHit rayHit;
    //private Color colorDefault;

    private bool buildingAlreadySelected;

    public GameObject currentSelectedBuilding;

    public string targetTag = "Grid";




    void Awake()
    {
       // colorDefault = grid[0].GetComponentInChildren<MeshRenderer>().material.color;
        buildings = GetComponent<Buildings>();

      

    }
    private void OnEnable()
    {
        // OnButtonCreateBuilding(1);
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Grid");
        grid = new GridElement[taggedObjects.Length];
        
        for (int i = 0; i < taggedObjects.Length; i++)
        {
            Debug.Log("TaggedObjects" + taggedObjects[i]);
            grid[i] = taggedObjects[i].GetComponent<GridElement>();
        }


    }


    void Update()
    {
//        Debug.Log(currentHoveredGridElement);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        if (Physics.Raycast(ray, out rayHit))
        {
            GridElement g = rayHit.transform.GetComponent<GridElement>();
            if (!g)
            {
                if (currentHoveredGridElement)
                {
                    currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = currentHoveredGridElement.colorDefault;
                    return;
                }

            }
            if (Input.GetMouseButtonDown(0))
            {
                currentSelectedGridElement = g;
            }
            if (g != currentHoveredGridElement)
            {
                if (!g.occupied)
                {
                    rayHit.transform.GetComponent<MeshRenderer>().material.color = colorHover;
                }
                else
                    rayHit.transform.GetComponent<MeshRenderer>().material.color = colorOnOccupied;
            }
            if (currentHoveredGridElement && currentHoveredGridElement != g)
            {
                Debug.Log("Set Default");
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = currentHoveredGridElement.colorDefault;

                
            }

            currentHoveredGridElement = g;



        }
        else
        {
        
            //if (currentHoveredGridElement)
            
//            currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = colorDefault;

        }
        MoveBuilding();
        PlaceBuilding();



    }
    public void OnButtonCreateBuilding(int id)
    {

            
        Debug.Log("Test1");
        if (buildingAlreadySelected)
            return;

        GameObject g = null;

        foreach (GameObject gO in buildings.buildabables)
        {
            Building b = gO.GetComponent<Building>();
            if (b.info.ID == id)
            {
                g = b.gameObject;
              
            }

        }

        Debug.Log("currentSelectedBuilding, " + currentSelectedBuilding);
        Vector3 spawnPos = new Vector3(5, 0, 0);
        currentSelectedBuilding = Instantiate(g, spawnPos, Quaternion.identity);
        


        info.DisplayInfo();

        currentSelectedBuilding.transform.rotation = Quaternion.Euler(0, 0, 0);
        buildingAlreadySelected = true;
    }
    public void MoveBuilding()
    {
        if (!currentSelectedBuilding)
            return;
        currentSelectedBuilding.gameObject.layer = 2;
        if (currentHoveredGridElement)
        {
            //currentSelectedBuilding.transform.position = currentHoveredGridElement.transform.position;
            currentSelectedBuilding.transform.position = new Vector3(currentHoveredGridElement.transform.position.x, currentHoveredGridElement.transform.position.y +0.4f, currentHoveredGridElement.transform.position.z);

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentSelectedBuilding);
                currentSelectedBuilding = null;

            }
            if (Input.GetMouseButton(2) || Input.GetKeyDown(KeyCode.R))
            {
                currentSelectedBuilding.transform.Rotate(transform.up * 30);
            }
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
            {
                currentSelectedBuilding.transform.Rotate(transform.up * -30);
            }

        }

    






    }
    public void PlaceBuilding()
    {
        buildings.builtObjects.Add(currentSelectedBuilding);
        Building b = currentSelectedBuilding.GetComponent<Building>();
        if (!currentSelectedBuilding || currentHoveredGridElement.occupied || !currentHoveredGridElement.CompareTag(b.requiredTag))
            return;

       if (Input.GetMouseButtonDown(0))
        {
            bool canPlace = b.requiredBuilding == null;
            foreach (Transform T in FindObjectsOfType<Transform>())
            {

                if (string.IsNullOrEmpty(b.requiredBuilding) || (T.name == b.requiredBuilding && Vector3.Distance(transform.position, T.position) <= b.requiredDistanceFromBuilding))

                {
                    canPlace = true;
                    break;
                }


            }
            if (canPlace)
            {
                currentHoveredGridElement.occupied = true;
                currentHoveredGridElement.connectedBuilding = b;
                b.placed = true;
                b.info.connectedGridID = currentHoveredGridElement.gridID;
                b.info.yRotation = b.transform.localEulerAngles.y;
                b.ConstructBuilding();
                currentSelectedBuilding = null;
                buildingAlreadySelected = false;
            }
            else
            {
                Debug.LogWarning("Cannot place building: no required building found within the required distance.");
            }


        }
    }
    public void RebuildBuilding(int builingID, int gridID, float buildingLevel, float rotY)
    {
        GameObject g = null;
        foreach(GameObject g0 in buildings.buildabables)
        {
            Building b = g0.GetComponent<Building>();
            if(b.info.ID == builingID)
            {
                g =  b.gameObject;
            }

        }
        GameObject building = Instantiate(g);
        
        buildings.builtObjects.Add(building);
        Building loadedBuilding = building.GetComponent<Building>();
        loadedBuilding.info.buildingLevel = buildingLevel;
        loadedBuilding.placed = true;
        loadedBuilding.info.connectedGridID = gridID;
        

        GridElement myElement = grid[gridID].GetComponent<GridElement>();
        building.transform.position = new Vector3(myElement.transform.position.x, 0.356f, myElement.transform.position.z);
        building.transform.rotation = Quaternion.Euler(0, rotY, 0);
        loadedBuilding.info.yRotation = rotY;

        myElement.occupied = true;
        myElement.connectedBuilding = loadedBuilding;
        
    }

}
