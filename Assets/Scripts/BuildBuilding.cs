using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBuilding : MonoBehaviour
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
    private bool buildingAlreadySelected;
    public GameObject currentSelectedBuilding;
    public string targetTag = "Grid";

    void Awake()
    {
        buildings = GetComponent<Buildings>();
    }

    private void OnEnable()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Grid");
        grid = new GridElement[taggedObjects.Length];

        for (int i = 0; i < taggedObjects.Length; i++)
        {
            grid[i] = taggedObjects[i].GetComponent<GridElement>();
        }
    }

    void Update()
    {
        HandleHover();
        MoveBuilding();
        PlaceBuilding();
    }

    private void HandleHover()
    {
        // Don't let clicks on UI (buttons, panels, etc.) hit the world underneath
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (currentHoveredGridElement != null)
            {
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color =
                    currentHoveredGridElement.colorDefault;
                currentHoveredGridElement = null;
            }
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out rayHit))
        {
            if (currentHoveredGridElement != null)
            {
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color =
                    currentHoveredGridElement.colorDefault;
                currentHoveredGridElement = null;
            }
            return;
        }

        GridElement g = rayHit.transform.GetComponent<GridElement>();

        if (g == null)
        {
            if (currentHoveredGridElement != null)
            {
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color =
                    currentHoveredGridElement.colorDefault;
                currentHoveredGridElement = null;
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
            currentSelectedGridElement = g;

        if (g != currentHoveredGridElement)
        {
            g.GetComponent<MeshRenderer>().material.color = g.occupied ? colorOnOccupied : colorHover;

            if (currentHoveredGridElement != null)
            {
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color =
                    currentHoveredGridElement.colorDefault;
            }

            currentHoveredGridElement = g;
        }
    }

    public void OnButtonCreateBuilding(int id)
    {
        if (buildingAlreadySelected)
            return;

        GameObject prefab = FindBuildingPrefabByID(id);
        if (prefab == null)
        {
            Debug.LogWarning($"No building prefab found with ID {id}");
            return;
        }

        Vector3 spawnPos = new Vector3(5, 0, 0);
        currentSelectedBuilding = Instantiate(prefab, spawnPos, Quaternion.identity);

       info.DisplayInfo(currentSelectedBuilding.GetComponent<TowerData>());
        buildingAlreadySelected = true;
    }

    public void MoveBuilding()
    {
        if (currentSelectedBuilding == null)
            return;

        currentSelectedBuilding.layer = 2;

        if (currentHoveredGridElement == null)
            return;

        currentSelectedBuilding.transform.position = new Vector3(
            currentHoveredGridElement.transform.position.x,
            currentHoveredGridElement.transform.position.y + 0.4f,
            currentHoveredGridElement.transform.position.z
        );

        // Right-click cancels placement
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentSelectedBuilding);
            currentSelectedBuilding = null;
            buildingAlreadySelected = false;
            return;
        }

        // R rotates 30 degrees; Shift+R rotates -30 degrees
        if (Input.GetKeyDown(KeyCode.R))
        {
            float direction = Input.GetKey(KeyCode.LeftShift) ? -30f : 30f;
            currentSelectedBuilding.transform.Rotate(Vector3.up * direction);
        }
        // Middle-mouse also rotates forward 30 degrees
        else if (Input.GetMouseButtonDown(2))
        {
            currentSelectedBuilding.transform.Rotate(Vector3.up * 30f);
        }
    }

    public void PlaceBuilding()
    {
        if (currentSelectedBuilding == null)
            return;

        if (currentHoveredGridElement == null)
            return;

        Building b = currentSelectedBuilding.GetComponent<Building>();
        if (b == null)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        // Tile must be free
        if (currentHoveredGridElement.occupied)
            return;

        // Commit placement
        buildings.builtObjects.Add(currentSelectedBuilding);

        currentHoveredGridElement.occupied = true;
        currentHoveredGridElement.connectedBuilding = b;

        b.placed = true;
        b.info.connectedGridID = currentHoveredGridElement.gridID;
        b.info.yRotation = b.transform.localEulerAngles.y;
        b.ConstructBuilding();

        // Restore layer so future raycasts hit it
        currentSelectedBuilding.layer = 0;

        currentSelectedBuilding = null;
        buildingAlreadySelected = false;
    }

    public void RebuildBuilding(int buildingID, int gridID, float buildingLevel, float rotY)
    {
        GameObject prefab = FindBuildingPrefabByID(buildingID);
        if (prefab == null)
        {
            Debug.LogWarning($"RebuildBuilding: no prefab found for ID {buildingID}");
            return;
        }

        GameObject building = Instantiate(prefab);
        buildings.builtObjects.Add(building);

        Building loadedBuilding = building.GetComponent<Building>();
        loadedBuilding.info.buildingLevel = buildingLevel;
        loadedBuilding.placed = true;
        loadedBuilding.info.connectedGridID = gridID;

        GridElement myElement = grid[gridID];
        building.transform.position = new Vector3(
            myElement.transform.position.x,
            0.356f,
            myElement.transform.position.z
        );
        building.transform.rotation = Quaternion.Euler(0, rotY, 0);
        loadedBuilding.info.yRotation = rotY;

        myElement.occupied = true;
        myElement.connectedBuilding = loadedBuilding;
    }
    private GameObject FindBuildingPrefabByID(int id)
    {
        foreach (GameObject go in buildings.buildabables)
        {
            BuildingInfo info = go.GetComponent<BuildingInfo>();
            if (info != null && info.ID == id)
                return go;
        }
        return null;
    }
}