using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TowerEquipManager : MonoBehaviour
{
    public int towerLimit = 5;
    public Info info;
    public int equippedCount;
    public TextMeshProUGUI equippedText;
    public TextMeshProUGUI equippedCountText;
    public List<Tower> equippedTowers = new();
    public List<GameObject> towerClasses = new();
    public List<Transform> slotTransform = new();
    public List<GameObject> equippedButtons = new();

    public void AddTower()
    {
        Tower tower = info.selectedTower;
        if (equippedCount >= towerLimit) return;
        if (tower == null) return;
        if (equippedTowers.Contains(tower)) return;

        
        equippedCountText.text = $"Towers: ({equippedCount}/5)";
        equippedTowers.Add(tower);
        GameObject buttonObject = Instantiate(tower.buttonPrefab, slotTransform[equippedCount]);
        equippedButtons.Add(buttonObject);
        
        equippedCount++;
        for (int i = index; i < equippedButtons.Count; i++)
        {
            equippedButtons[i].tranform.SetParent(slotTransform[i], false);
        }
    }
    public void RemoveTower()
    {
        Tower tower = info.selectedTower;
        if (tower == null) return;
        int index = equippedButtons.IndexOf(tower);
        Destroy(equippedButtons[index])
        equippedTowers.RemoveAt(index);
        equippedButtons.RemoveAt(index);
        equippedCount--;

        equippedCountText.text = $"Towers: ({equippedCount}/5)";
    }

    public void StartGame()
    {
        EquipLoadout.equippedTowers = equippedTowers
            .Select(t => t.TowerClassData)
            .ToList();
        SceneManager.LoadScene("GameScene");
    }

    public void SelectClass(int i)
    {
        for (int g = 0; g < towerClasses.Count; g++)
        {
            towerClasses[g].SetActive(g == i);
        }


    }
    public void Update()
    {
        if (equippedCount >= towerLimit)
        {
            equippedCountText.color = Color.white;
        }
        else
            equippedCountText.color = Color.red;
    }

}

