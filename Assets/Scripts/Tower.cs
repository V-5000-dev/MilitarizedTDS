using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private TowerClass towerClass;

    
    public const float rotationSpeed = 10f;
    public const float projectileSpeed = 10f;

    public TowerData towerData;
    public TowerClass TowerClassData => towerClass;
    public TowerClass NextTier => towerClass.nextTier;
  
    public Sprite towerRank => towerClass.towerRank;
    public string TowerName => towerClass.towerName;
    public string TowerDisc => towerClass.towerDisc;
    public string UpgradeDisc => towerClass.upgradeDisc;
    private int UpgradeCost => towerClass.upgradeCost;
    public GameObject ButtonPrefab => towerClass.buttonPrefab;
    public float Damage => towerClass.damage;
    public float FireRate => towerClass.fireRate;
    public float Range => towerClass.range;
    public int Cost => towerClass.cost;
    public int MagSize => towerClass.magSize;

    public float OverTimeDmg => towerClass.overTimeDmg;
    public float OverTimeDuration => towerClass.overTimeDuration;
    public float ReloadSpeed => towerClass.reloadSpeed;
    public float SplashRange => towerClass.splashRange;
    public float SplashDamage => towerClass.spashDamage;
    public float CritChance => towerClass.critChance;
    public float CritDamage => towerClass.critDamage;
    public float CritSplashDamage => towerClass.critSplashDamage;
    public float CritSplashRange => towerClass.critSplashRange;
    public float CritOverTimeDmg => towerClass.critOverTimeDmg;
    public float CritOverTimeDuration => towerClass.critOverTimeDuration;

    public int ArmorPen => towerClass.armorPen;
    public bool RoundsReload => towerClass.roundsReload;

    public IReadOnlyList<TowerTag> Tags => towerClass.tags;

    protected virtual void Awake()
    {
        towerData = GetComponent<TowerData>();
        if (towerClass != null)
            OnClassApplied();
    }

    public void ApplyClass(TowerClass newClass)
    {
        if(MoneyController.money < newClass.cost)
        {
            StartCoroutine(MoneyController.NotEnoughMoney());
            return;
        }
        else
        {
            MoneyController.RemoveMoney(newClass.cost);
            towerClass = newClass;
            towerData.towerImage.sprite = newClass.towerRank;
        }
        OnClassApplied();
    }

    protected virtual void OnClassApplied() { }
}