using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UiHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
    public GameObject towerTags;
    public Info info;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tower t = info.selectedTower;
        if (t == null) return;

        text.text =
            $"Damage: {t.Damage}\n" +
            $"Armor Pen: {t.ArmorPen}\n" +
            $"Fire Rate: {t.FireRate}\n" +
            $"Range: {t.Range}\n" +
            $"Mag Size: {t.MagSize}\n" +
            $"Reload Speed: {t.ReloadSpeed}\n" +
            $"Crit Chance: {t.CritChance}\n" +
            $"Crit Damage: {t.CritDamage}\n" +
            $"Splash Damage: {t.SplashDamage}\n" +
            $"Splash Range: {t.SplashRange}\n" +
            $"Crit Splash Damage: {t.CritSplashDamage}\n" +
            $"Crit Splash Range: {t.CritSplashRange}";

        towerTags.SetActive(false);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        Tower t = info.selectedTower;
        if (t == null) return;

        text.text =
            $"Damage: {t.Damage}\n" +
            $"Armor Pen: {t.ArmorPen}\n" +
            $"Fire Rate: {t.FireRate}\n" +
            $"Range: {t.Range}\n" +
            $"Mag Size: {t.MagSize}\n" +
            $"Reload Speed: {t.ReloadSpeed}\n";
        
        towerTags.SetActive(true);
    }
}
