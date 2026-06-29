using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerClass towerClass;

    private string towerName;
    private string towerDisc;
    private Sprite icon;
    private float damage;
    private float fireRate;
    private float range;
    private int cost;
    private int magSize;
    private float reloadSpeed;
    private int armorPen;
    private bool hiddenDetect;

    public const float rotationSpeed = 10f;
    public const float projectileSpeed = 10f;

    public TowerClass TowerClassData => towerClass;
    public string TowerName => towerName;
    public string TowerDisc => towerDisc;
    public Sprite Icon => icon;
    public float Damage => damage;
    public float FireRate => fireRate;
    public float Range => range;
    public int Cost => cost;
    public int MagSize => magSize;
    public float ReloadSpeed => reloadSpeed;
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
        towerDisc = newClass.towerDisc;
        towerClass = newClass;
        damage = newClass.damage;
        fireRate = newClass.fireRate;
        range = newClass.range;
        cost = newClass.cost;
        magSize = newClass.magSize;
        reloadSpeed = newClass.reloadSpeed;
        armorPen = newClass.armorPen;
        hiddenDetect = newClass.hiddenDetect;
    }
}