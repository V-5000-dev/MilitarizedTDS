
using UnityEngine.UI;
using UnityEngine;

public class TowerData : Tower
{
    public Image towerImage;

    protected override void OnClassApplied()
    {
        if (towerImage != null) towerImage.sprite = TowerIcon;
    }
}