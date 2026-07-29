using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Info : MonoBehaviour
{
    public Button btnDestory;
    public Button btnUpgrade;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI discText;
    public TextMeshProUGUI statsText;
    public Image[] levelImages;
    public Image towerlevel;
    public TowerInfoUI towerInfoUI;
    public GameObject upgradePanel;
    public GameObject infoPanel;
    public TextMeshProUGUI upgradeTitle;
    public TextMeshProUGUI upgradeDisc;
    public TextMeshProUGUI upgradeText;
    public Button upgradeConfirm;


    private BuildBuilding build;
    private Building selectedBuilding;
    public Tower selectedTower;

    // Track last selection so we only redraw when it changes
    private TowerData _lastDisplayedTower;
    private bool _inPreviewMode;

    void Awake()
    {
        build = FindObjectOfType<BuildBuilding>();
    }

    void Update()
    {
        if (build == null)
        {
            btnDestory.interactable = false;
            return;
        }

        TowerData newSelection = null;
        if (build.currentSelectedGridElement != null && build.currentSelectedGridElement.connectedBuilding != null)
            newSelection = build.currentSelectedGridElement.connectedBuilding.GetComponent<TowerData>();
        else if (build.currentSelectedBuilding != null)
            newSelection = build.currentSelectedBuilding.GetComponent<TowerData>();

        // A real in-world selection overrides preview mode
        if (newSelection != null)
            _inPreviewMode = false;

        // Don't let the polling loop clear a preview set by ShowPreview()
        if (_inPreviewMode)
        {
            btnDestory.interactable = false;
            return;
        }

        if (newSelection != _lastDisplayedTower)
        {
            _lastDisplayedTower = newSelection;

            if (newSelection != null)
            {
                DisplayInfo(newSelection);
            }
            else
            {
                selectedBuilding = null;
                selectedTower = null;
                nameText.text = "No Building Selected.";
                discText.text = " ";
                towerInfoUI.Hide();
            }
        }

        btnDestory.interactable = selectedBuilding != null;
    }

    public void OnBtnDestory()
    {
        if (selectedBuilding == null) return;

        build.currentSelectedGridElement.occupied = false;
        build.buildings.builtObjects.Remove(selectedBuilding.gameObject);
        Destroy(selectedBuilding.gameObject);

        // Reset so the UI clears after destruction
        _lastDisplayedTower = null;
    }
    public void OnBtnUpgrade()
    {
        if (selectedBuilding == null) return;

        upgradePanel.SetActive(true);
        infoPanel.SetActive(false);
        selectedTower = selectedBuilding.GetComponent<TowerData>();
        if (selectedTower.NextTier == null) return;

        TowerClass nextTier = selectedTower.NextTier;

        upgradePanel.SetActive(true);
        upgradeTitle.text = nextTier.towerName;
        upgradeDisc.text = nextTier.upgradeDisc;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        AppendStatChange(sb, "Damage", selectedTower.Damage, nextTier.damage);
        AppendStatChange(sb, "Armor Pen", selectedTower.ArmorPen, nextTier.armorPen);
        AppendStatChange(sb, "Fire Rate", selectedTower.FireRate, nextTier.fireRate);
        AppendStatChange(sb, "Range", selectedTower.Range, nextTier.range);
        AppendStatChange(sb, "Mag Size", selectedTower.MagSize, nextTier.magSize);
        AppendStatChange(sb, "Reload Speed", selectedTower.ReloadSpeed, nextTier.reloadSpeed);
        AppendStatChange(sb, "Dmg Over Time", selectedTower.OverTimeDmg, nextTier.overTimeDmg);
        AppendStatChange(sb, "Dmg Over T Duration", selectedTower.OverTimeDuration, nextTier.overTimeDuration);
        AppendStatChange(sb, "Splash Damage", selectedTower.SplashDamage, nextTier.splashRange);
        AppendStatChange(sb, "Splash Range", selectedTower.SplashRange, nextTier.splashRange);
        AppendStatChange(sb, "Critical Chance", selectedTower.CritChance, nextTier.critChance);
        AppendStatChange(sb, "Critical Damage", selectedTower.CritDamage, nextTier.critDamage);
        AppendStatChange(sb, "Critical Splash Damage", selectedTower.CritSplashDamage, nextTier.critSplashDamage);
        AppendStatChange(sb, "Critical Splash Range", selectedTower.CritSplashRange, nextTier.critSplashRange);
        AppendStatChange(sb, "Critical Dmg Over T", selectedTower.CritOverTimeDmg, nextTier.critOverTimeDmg);
        AppendStatChange(sb, "Critical Dmg Over T Duration", selectedTower.CritOverTimeDuration, nextTier.critOverTimeDuration);

        upgradeText.text = sb.ToString();

        towerInfoUI.ShowUpgradeTags(selectedTower);
    }
    public void OnBtnConfirm()
    {
        if (selectedTower == null || selectedTower.NextTier == null) return;
        selectedTower.ApplyClass(selectedTower.NextTier);
        _lastDisplayedTower = null;
        upgradePanel.SetActive(false);
        infoPanel.SetActive(true);
    }
    public void OnBtnReturn()
    {
        upgradePanel.SetActive(false);
        infoPanel.SetActive(true);
    }


    private void AppendStatChange(System.Text.StringBuilder sb, string label, float oldValue, float newValue)
    {
        if (Mathf.Approximately(oldValue, newValue)) return;
        sb.AppendLine($"{label}: {oldValue} -> {newValue}");
    }

    private void AppendStatChange(System.Text.StringBuilder sb, string label, int oldValue, int newValue)
    {
        if (oldValue == newValue) return;
        sb.AppendLine($"{label}: {oldValue} -> {newValue}");
    }

    private void AppendStatChange(System.Text.StringBuilder sb, string label, bool oldValue, bool newValue)
    {
        if (oldValue == newValue) return;
        sb.AppendLine($"{label}: {oldValue} -> {newValue}");
    }



    public void ShowPreview(TowerData tower)
    {
        // Prefab assets skip Awake(), so ApplyClass() may never have run
        if (string.IsNullOrEmpty(tower.TowerName) && tower.TowerClassData != null)
            tower.ApplyClass(tower.TowerClassData);

        _inPreviewMode = true;
        _lastDisplayedTower = tower;
        DisplayInfo(tower);
    }

    public void DisplayInfo(TowerData tower)
    {
        Debug.Log($"DisplayInfo called | tower={tower} | name={tower?.TowerName}");
        selectedBuilding = tower.GetComponent<Building>();
        selectedTower = tower;

        nameText.text = selectedTower.TowerName;
        discText.text = selectedTower.TowerDisc;
        statsText.text =
                $"Damage: {selectedTower.Damage}\n" +
                $"Armor Pen: {selectedTower.ArmorPen}\n" +
                $"Fire Rate: {selectedTower.FireRate}\n" +
                $"Range: {selectedTower.Range}\n" +
                $"Mag Size: {selectedTower.MagSize}\n" +
                $"Reload Speed: {selectedTower.ReloadSpeed}\n";

        towerInfoUI.ShowTower(selectedTower);


    }
}
