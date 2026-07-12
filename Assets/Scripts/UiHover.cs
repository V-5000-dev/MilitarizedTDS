using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UiHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
    public GameObject towerTags;
    public Info info;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tower t = info.selectedTower;
        if (t == null) return;

        var stats = new List<(string lablel, float value)>
        {
            ("Fire Rate", t.FireRate),
            ("Range", t.Range),
            ("Mag Size", t.MagSize),
            ("Reload Speed", t.ReloadSpeed),
            ("Dmg over Time", t.OverTimeDmg),
            ("Dmg Over T Duration", t.OverTimeDuration),
            ("Splash Damage", t.SplashDamage),
            ("Splash Range", t.SplashRange),
            ("Critical Chance", t.CritChance),
            ("Critical Damage", t.CritDamage),
            ("Crit Splash Damage", t.CritSplashDamage),
            ("Crit Splash Range", t.CritSplashRange),
            ("Crit Dmg Over Time", t.CritOverTimeDmg),
            ("Crit Dmg Over T Duation", t.CritOverTimeDuration),

        };
        string result = $"Damage: {t.Damage}\n" +
        $"Armor Pen: {t.ArmorPen}\n" +
        string.Join("\n", stats.Where(s => s.value != 0).Select(s => $"{s.lablel}: {s.value}"));
        text.text = result;
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
