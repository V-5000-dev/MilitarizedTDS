using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerEquipManager : MonoBehaviour
{
    public int towerLimit = 5;
    public List<Tower> equippedTowers = new List<Tower>();
    public void AddTower(Tower tower)
    {
        if (equippedTowers.Contains(tower))
            return;
        if (equippedTowers.Count >= towerLimit)
            equippedTowers.RemoveAt(0); 
            
        equippedTowers.Add(tower);
    }
}
