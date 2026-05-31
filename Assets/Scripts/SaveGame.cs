using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SavedProfile
{
    public float sv_wood, sv_stones, sv_food;
    public List<BuildingInfoSaveData> buildingsSVData = new List<BuildingInfoSaveData>();
    
}
public class SaveGame : MonoBehaviour
{
    public SavedProfile profile;
    private Resources resources;
    private Buildings buildings;
    private BuildBuilidng build;
    


    void Awake()
    {
        build = FindObjectOfType<BuildBuilidng>();
        buildings = FindObjectOfType<Buildings>();
        //LoadGame();
    }

    
    private void Save()
    {
        if (profile == null)
            profile = new SavedProfile();
;

        foreach(GameObject g in buildings.builtObjects)
        {
            BuildingInfo info = g.GetComponent<BuildingInfo>();
            profile.buildingsSVData.Add(info.GetSaveData());
            // should be .info at the end of the previous line
           

         
        }
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.dat";
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        bf.Serialize(fs, profile);

        fs.Close();
        
    }
    private void LoadGame()
    {
        string pathToLoad = Application.persistentDataPath + "/save.dat";
        if (!File.Exists(pathToLoad))
        {
            Debug.Log("No save profile found.");
            return;      
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(pathToLoad, FileMode.Open);
        SavedProfile loadedProfile = bf.Deserialize(fs) as SavedProfile;
        fs.Close();



        for (int i = 0; i < loadedProfile.buildingsSVData.Count; i++)
        {
            BuildingInfoSaveData buildingFromSave = loadedProfile.buildingsSVData[i];
            build.RebuildBuilding(buildingFromSave.bisd_ID, buildingFromSave.bisd_connectedGridID, buildingFromSave.bisd_buildingLevel, buildingFromSave.bisd_yRot);
            Debug.Log(buildingFromSave.bisd_ID);
        }






    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Save");
            Save();
        }    
    }

}

