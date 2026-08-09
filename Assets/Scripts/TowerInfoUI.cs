using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TowerInfoUI : MonoBehaviour
{
    [Header("Panel Root")]
    public GameObject panelRoot;

    [Header("Tower Info Text")]
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI towerDescriptionText;

    [Header("Tower Icon")]

    [Header("Tag Icons")]
    public GameObject tagIconPrefab;
    public Transform tagIconContainer;

    [Header("Upgrade Tag Icons")]
    public Transform upgradeTagIconContainer;

    [Header("Tooltip")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipNameText;
    public TextMeshProUGUI tooltipDescriptionText;
    public Vector2 tooltipOffset = new Vector2(10f, 10f);

    private readonly List<GameObject> _spawnedIcons = new List<GameObject>();
    private readonly List<GameObject> _spawnedUpgradeIcons = new List<GameObject>();
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();

        panelRoot.SetActive(false);

        tooltipPanel.SetActive(false);
        foreach (var graphic in tooltipPanel.GetComponentsInChildren<Graphic>(true))
            graphic.raycastTarget = false;
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
            PositionTooltipAtMouse();
    }

    public void ShowTower(Tower tower)
    {
        if (tower == null) { Hide(); return; }

 //       towerNameText.text        = tower.TowerName;
    //    towerDescriptionText.text = tower.TowerDisc;


        var srcImage = tower.GetComponentInChildren<Image>();


        // Activate the panel FIRST so the hierarchy (including tagIconContainer) is active
        panelRoot.SetActive(true);

        // THEN spawn tag icons, so their Awake() runs correctly
        RebuildTagIcons(tower.Tags, tagIconContainer, _spawnedIcons);
    }

    public void ShowUpgradeTags(Tower tower)
    {
        if (tower == null)
        {
            RebuildTagIcons(System.Array.Empty<TowerTag>(), upgradeTagIconContainer, _spawnedUpgradeIcons);
            return;
        }

        RebuildTagIcons(tower.NextTier.tags, upgradeTagIconContainer, _spawnedUpgradeIcons);
    }

    public void Hide()
    {
        panelRoot.SetActive(false);
        HideTooltip();
    }

    public void ShowTooltip(TowerTag tag)
    {
        if (tag == null) return;

        tooltipNameText.text        = tag.tagName;
        tooltipDescriptionText.text = tag.description;

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.SetAsLastSibling();
        PositionTooltipAtMouse();
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private void RebuildTagIcons(IReadOnlyList<TowerTag> tags, Transform container, List<GameObject> spawnedIcons)
    {
        foreach (GameObject icon in spawnedIcons)
            Destroy(icon);
        spawnedIcons.Clear();

        foreach (TowerTag tag in tags)
        {
            if (tag == null) continue;

            GameObject iconGO = Instantiate(tagIconPrefab, container);
            iconGO.name = $"TagIcon_{tag.tagName}";

            TagIconUI iconUI = iconGO.GetComponent<TagIconUI>();
            iconUI.Initialize(tag, this);

            spawnedIcons.Add(iconGO);
        }
    }

    private void PositionTooltipAtMouse()
    {
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        RectTransform parentRect  = tooltipRect.parent as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out localPoint
        );

        // localPosition marks the rect's pivot, not its bottom-left corner.
        // Offset by the pivot so the tooltip's bottom-left corner sits at the mouse + offset,
        // making the tooltip expand up and to the right of the cursor regardless of pivot setting.
        Vector2 pivotOffset = new Vector2(
            tooltipRect.rect.width  * tooltipRect.pivot.x,
            tooltipRect.rect.height * tooltipRect.pivot.y
        );
        tooltipRect.localPosition = localPoint + tooltipOffset + pivotOffset;

        RectTransform canvasRect = _canvas.transform as RectTransform;
        Vector3[] corners       = new Vector3[4];
        Vector3[] canvasCorners = new Vector3[4];
        tooltipRect.GetWorldCorners(corners);
        canvasRect.GetWorldCorners(canvasCorners);

        float overflowRight = corners[2].x - canvasCorners[2].x;
        float overflowTop   = corners[2].y - canvasCorners[2].y;

        Vector3 pos = tooltipRect.localPosition;
        if (overflowRight > 0) pos.x -= overflowRight;
        if (overflowTop   > 0) pos.y -= overflowTop;
        tooltipRect.localPosition = pos;
    }
}
