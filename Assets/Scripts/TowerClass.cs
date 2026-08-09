using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerClass", menuName = "Tower Defense/Tower Class")]
public class TowerClass : ScriptableObject
{
    [Header("TowerName")]
    public string towerName;

    [TextArea(2, 4)]
    public string towerDisc;
    public string upgradeDisc;

    [Header("Visuals")]
    public GameObject buttonPrefab;

    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float minRange;
    public float range;
    public float accuracy;
    public int cost;
    public int rankUnlock;
    public int magSize;
    public float reloadSpeed;
    public bool roundsReload;
    public int armorPen = 0;
    public float overTimeDmg;
    public float overTimeDuration;
    public float splashRange;
    public float spashDamage;
    public float critChance;
    public float critDamage;
    public float critSplashDamage;
    public float critSplashRange;
    public float critOverTimeDmg;
    public float critOverTimeDuration;

    
    public bool hiddenDetect = false;

    [Header("Display Tags")]
    public List<TowerTag> tags = new List<TowerTag>();
    [Header("Upgrade")]
    public TowerClass nextTier;
    [Tooltip("Cost to upgrade into nextTier.")]
    public int upgradeCost;

    public bool HasUpgrade => nextTier != null;
   
}