using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuildingInfo: MonoBehaviour
{
    public BuildingInfoSaveData buidlingInfoSaves;
    public int ID;
    public float buildingLevel = 0;
    public float baseCost;
    public float yRotation = 0;
    public int connectedGridID;
    public BuildingInfoSaveData GetSaveData()
    {
        return new BuildingInfoSaveData
        {
            bisd_connectedGridID = connectedGridID,
            bisd_ID = ID,
            bisd_buildingLevel = buildingLevel,
            position = new SerializableVector3(transform.position),
            rotation = new SerializableQuaternion(transform.rotation),
            bisd_yRot = yRotation,
        };
    }

}
[System.Serializable]
public class BuildingInfoSaveData
{
    public float bisd_buildingLevel = 0;
    public float bisd_buildingRecourceProduction;
    public int bisd_ID;
    public int bisd_connectedGridID;
    public float bisd_yRot;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;



}
[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}

