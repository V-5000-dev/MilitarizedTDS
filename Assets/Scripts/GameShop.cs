using UnityEngine;

public class GameShop : MonoBehaviour
{
    public GameObject shopButtonPrefab;
    public Transform[] slots;

    void Start()
    {
        var loadout = EquipLoadout.equippedTowers;
        Buildings buildings = FindObjectOfType<Buildings>();

        for (int i = 0; i < slots.Length; i++)
        {
            foreach (Transform child in slots[i])
                Destroy(child.gameObject);

            if (i >= loadout.Count || buildings == null) continue;

            TowerClass targetClass = loadout[i];
            int buildingID = FindBuildingID(buildings, targetClass);
            if (buildingID < 0) continue;

            GameObject btn = Instantiate(shopButtonPrefab, slots[i]);
            btn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            btn.GetComponent<BuyButton>().connectedBuildingID = buildingID;
        }
    }

    private int FindBuildingID(Buildings buildings, TowerClass targetClass)
    {
        foreach (GameObject buildable in buildings.buildabables)
        {
            TowerData td = buildable.GetComponent<TowerData>();
            if (td != null && td.TowerClassData == targetClass)
            {
                BuildingInfo info = buildable.GetComponent<BuildingInfo>();
                if (info != null) return info.ID;
            }
        }
        return -1;
    }
}
