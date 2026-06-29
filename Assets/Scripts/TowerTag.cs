using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerTag", menuName = "Tower Defense/Tower Tag")]
public class TowerTag : ScriptableObject
{
    [Header("Tag Identity")]
    public string tagName;

    [TextArea(2, 4)]
    public string description;

    [Header("Visuals")]
    public Sprite icon;
}