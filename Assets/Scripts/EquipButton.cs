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

}
