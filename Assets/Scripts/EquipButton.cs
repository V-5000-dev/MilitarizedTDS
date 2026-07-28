using UnityEngine;

public class EquipButton : MonoBehaviour
{
    public Tower tower;
    public GameObject infoBar;
    private Info infoScript;
    public TowerEquipManager equipManager;
    public void OnButtonPressed()
    {
        infoScript = infoBar.GetComponent<Info>();
        infoScript.selectedTower = tower;
        infoScript.DisplayInfo(tower);

    }
    public void OnEquipPressed()
    {
        infoScript = infoBar.GetComponent<Info>();
        infoScript.selectedTower = tower;
        equipManager.AddTower(tower);

    }

}
