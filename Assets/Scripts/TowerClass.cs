using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerClass", menuName = "Tower Defense/Tower Class")]
public class TowerClass : ScriptableObject
{
    [Header("TowerName")]
    public string towerName;

    [TextArea(2, 4)]
    public string towerDisc;

    [Header("Visuals")]
    public Sprite icon;

    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float range;
    public int cost;
    public int magSize;
    public float reloadSpeed;
    public int armorPen = 0;
    public bool hiddenDetect = false;

    [Header("Display Tags")]
    [Tooltip("The tags shown on the tower's info screen (e.g. Semi-Automatic, Armor Pen II, Explosive).")]
    public List<TowerTag> tags = new List<TowerTag>();
}