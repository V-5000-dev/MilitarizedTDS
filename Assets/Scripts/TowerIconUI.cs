using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TagIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TowerTag _tagData;
    private TowerInfoUI _infoUI;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Initialize(TowerTag tagData, TowerInfoUI infoUI)
    {
        _tagData = tagData;
        _infoUI  = infoUI;

        _image.sprite = tagData.icon;
        _image.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_infoUI != null && _tagData != null)
            _infoUI.ShowTooltip(_tagData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_infoUI != null)
            _infoUI.HideTooltip();
    }
}