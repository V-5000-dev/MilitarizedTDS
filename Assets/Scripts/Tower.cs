using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerClass towerClass;

    [SerializeField] private string towerName;
    [SerializeField, TextArea(2, 4)] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float range;
    [SerializeField] private int cost;
    [SerializeField] private int magSize;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private int armorPen;
    [SerializeField] private bool hiddenDetect;

    public const float rotationSpeed = 10f;
    public const float projectileSpeed = 10f;

    public TowerClass TowerClassData => towerClass;
    public string TowerName => towerName;
    public string Description => description;
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