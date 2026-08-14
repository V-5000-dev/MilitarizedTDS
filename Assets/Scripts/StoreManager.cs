using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public List<Transform> slotTransform;
    public List<TowerClass> equipped;
    void Start()
    {
        SpawnStore();
    }
    void SpawnStore()
    {
        equipped = EquipLoadout.equippedTowers;
        for (int i = 0; i < slotTransform.Count && i < equipped.Count; i++)
        {
            TowerClass towerData = equipped[i];
            Debug.Log($"Instatate {towerData.buttonPrefab}");
            Instantiate(towerData.buttonPrefab, slotTransform[i]);
            
        }
    }

}
