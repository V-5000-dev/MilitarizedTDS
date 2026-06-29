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

    [Header("Tag Icons")]
    public GameObject tagIconPrefab;
    public Transform tagIconContainer;

    [Header("Tooltip")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipNameText;
    public TextMeshProUGUI tooltipDescriptionText;
    public Vector2 tooltipOffset = new Vector2(10f, 10f);

    private readonly List<GameObject> _spawnedIcons = new List<GameObject>();
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();

        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (tooltipPanel != null && tooltipPanel.activeSelf)
            PositionTooltipAtMouse();
    }

    public void ShowTower(Tower tower)
    {
        Debug.Log($"ShowTower called. tower={tower}, panelRoot={panelRoot}");
        if (tower == null) { Hide(); return; }

        // FIX: tower.name is the GameObject name — use tower.TowerName instead
        if (towerNameText != null)        towerNameText.text        = tower.TowerName;
        if (towerDescriptionText != null) towerDescriptionText.text = tower.Description;

        RebuildTagIcons(tower.Tags);

        if (panelRoot != null)
            panelRoot.SetActive(true);
    }

    public void Hide()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        HideTooltip();
    }

    public void ShowTooltip(TowerTag tag)
    {
        if (tooltipPanel == null || tag == null) return;

        tooltipNameText.text        = tag.tagName;
        tooltipDescriptionText.text = tag.description;

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.SetAsLastSibling();
        PositionTooltipAtMouse();
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    private void RebuildTagIcons(IReadOnlyList<TowerTag> tags)
    {
        foreach (GameObject icon in _spawnedIcons)
            Destroy(icon);
        _spawnedIcons.Clear();

        if (tagIconPrefab == null || tagIconContainer == null) return;

        foreach (TowerTag tag in tags)
        {
            if (tag == null) continue;

            GameObject iconGO = Instantiate(tagIconPrefab, tagIconContainer);
            iconGO.name = $"TagIcon_{tag.tagName}";

            TagIconUI iconUI = iconGO.GetComponent<TagIconUI>();
            if (iconUI != null)
                iconUI.Initialize(tag, this);

            _spawnedIcons.Add(iconGO);
        }
    }

    private void PositionTooltipAtMouse()
    {
        if (_canvas == null || tooltipPanel == null) return;

        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            Input.mousePosition,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out localPoint
        );

        tooltipRect.localPosition = localPoint + tooltipOffset;

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