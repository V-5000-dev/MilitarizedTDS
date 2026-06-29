using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Info : MonoBehaviour
{
    public Button btnDestory;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI discText;
    public TextMeshProUGUI statsText;
    public Image[] levelImages;
    public Image towerlevel;
    public TowerInfoUI towerInfoUI;

    private BuildBuilding build;
    private Building selectedBuilding;
    private Tower selectedTower;

    // Track last selection so we only redraw when it changes
    private Building _lastDisplayedBuilding;

    void Awake()
    {
        build = FindObjectOfType<BuildBuilding>();
    }

    void Update()
    {
        // Resolve what is currently selected
        Building newSelection = null;

        if (build.currentSelectedGridElement != null && build.currentSelectedGridElement.connectedBuilding != null)
            newSelection = build.currentSelectedGridElement.connectedBuilding;
        else if (build.currentSelectedBuilding != null)
            newSelection = build.currentSelectedBuilding.GetComponent<Building>();

        // Only update the UI when the selection actually changes
        if (newSelection != _lastDisplayedBuilding)
        {
            _lastDisplayedBuilding = newSelection;

            if (newSelection != null)
            {
                DisplayInfo(newSelection);
            }
            else
            {
                selectedBuilding = null;
                selectedTower    = null;
                nameText.text    = "No Building Selected.";
                discText.text    = " ";
                towerInfoUI.Hide();
            }
        }

        btnDestory.interactable = selectedBuilding != null;
    }

    public void OnBtnDestory()
    {
        if (selectedBuilding == null) return;

        build.currentSelectedGridElement.occupied = false;
        RefundResources();
        build.buildings.builtObjects.Remove(selectedBuilding.gameObject);
        Destroy(selectedBuilding.gameObject);

        // Reset so the UI clears after destruction
        _lastDisplayedBuilding = null;
    }

    public void RefundResources()
    {
        // TODO: implement refund logic
    }

    public void DisplayInfo(Building building)
    {
        Debug.Log($"DisplayInfo called | tower={selectedTower} | name={selectedTower?.TowerName}");
        selectedBuilding = building;
        selectedTower    = building.GetComponent<TowerData>();

        nameText.text = selectedTower.TowerName;
        discText.text = selectedTower.TowerName;


        statsText.text =
                $"Damage: {selectedTower.Damage}\n" +
                $"Armor Pen: {selectedTower.ArmorPen}\n" +
                $"Fire Rate: {selectedTower.FireRate}\n" +
                $"Range: {selectedTower.Range}\n" +
                $"Mag Size: {selectedTower.MagSize}\n" +
                $"Reload Speed: {selectedTower.ReloadSpeed}\n" +
                $"Hidden Detection: {selectedTower.HiddenDetect}";

        towerInfoUI.ShowTower(selectedTower);

        
    }
}