using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerClass towerClass;
    private TowerClass nextTier;

    private int upgradeCost;

    private string towerName;
    private string towerDisc;
    private string upgradeDisc;
    private Sprite towerIcon;
    private float damage;
    private float fireRate;
    private float range;
    private int cost;
    private int magSize;
    private float reloadSpeed;
    private float spashRange;
    private float spashDamage;
    private float critChance;
    private float critDamage;
    private float critSplashDamage;
    private float critSplashRange;
    private int armorPen;
    private bool hiddenDetect;

    public const float rotationSpeed = 10f;
    public const float projectileSpeed = 10f;

    public TowerClass TowerClassData => towerClass;
    public TowerClass NextTier => nextTier;

    public string TowerName => towerName;
    public string TowerDisc => towerDisc;
    public string UpgradeDisc => upgradeDisc;

    private int UpgradeCost => upgradeCost;
    public Sprite TowerIcon => towerIcon;
    public float Damage => damage;
    public float FireRate => fireRate;
    public float Range => range;
    public int Cost => cost;
    public int MagSize => magSize;
    public float ReloadSpeed => reloadSpeed;
    public float SplashRange => spashRange;
    public float SplashDamage => spashDamage;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public float CritSplashDamage => critSplashDamage;
    public float CritSplashRange => critSplashRange;

    public int ArmorPen => armorPen;
    public bool HiddenDetect => hiddenDetect;

    public IReadOnlyList<TowerTag> Tags =>
        towerClass != null ? towerClass.tags : System.Array.Empty<TowerTag>();

    protected virtual void Awake()
    {
        if (towerClass != null)
            ApplyClass(towerClass);
    }

    public void ApplyClass(TowerClass newClass)
    {
        towerName = newClass.towerName;
        towerIcon = newClass.towerIcon;
        nextTier = newClass.nextTier;
        upgradeCost = newClass.upgradeCost;
        towerDisc = newClass.towerDisc;
        upgradeDisc = newClass.upgradeDisc;
        towerClass = newClass;
        damage = newClass.damage;
        fireRate = newClass.fireRate;
        range = newClass.range;
        cost = newClass.cost;
        magSize = newClass.magSize;
        reloadSpeed = newClass.reloadSpeed;
        armorPen = newClass.armorPen;
        spashRange = newClass.splashRange;
        spashDamage = newClass.spashDamage;
        critChance = newClass.critChance;
        critDamage = newClass.critDamage;
        critSplashDamage = newClass.critSplashDamage;
        critSplashRange = newClass.critSplashRange;

        OnClassApplied();
    }

    protected virtual void OnClassApplied() { }
}