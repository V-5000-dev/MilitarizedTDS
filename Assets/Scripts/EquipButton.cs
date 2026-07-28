using UnityEngine;

public class EquipButton : MonoBehaviour
{
    public TowerData towerClass;
    public GameObject infoBar;
    private Info infoScript;
    public TowerEquipManager equipManager;
    public void OnButtonPressed()
    {
        infoScript = infoBar.GetComponent<Info>();
        infoScript.ShowPreview(towerClass);
    }
    public void OnEquipPressed()
    {
        infoScript = infoBar.GetComponent<Info>();
        infoScript.selectedTower = towerClass;
        equipManager.AddTower(towerClass);

    }

}
