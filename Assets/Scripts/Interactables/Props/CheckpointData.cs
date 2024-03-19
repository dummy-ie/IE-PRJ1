using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CheckpointData", menuName = "Scriptable Objects/CheckpointData")]
[Serializable]
[JsonObject]
public class CheckpointData
{

    [JsonProperty]
    [SerializeField]
    private string _checkPointName;
    public string CheckPointName {
        get { return this._checkPointName; }
        set { this._checkPointName = value; }
    }


    [JsonProperty]
    [SerializeField]
    private float _posX;
    public float PosX { get { return _posX; } }

    [JsonProperty]
    [SerializeField]
    private float _posY;
    public float PosY { get { return _posY; } }

    [JsonProperty]
    [SerializeField]
    private float _posZ;
    public float PosZ { get { return _posZ; } }

    public void SetRespawnPos(Vector3 respawnPos)
    {
        _posX = respawnPos.x;
        _posY = respawnPos.y;  
        _posZ = respawnPos.z;
    }


}
