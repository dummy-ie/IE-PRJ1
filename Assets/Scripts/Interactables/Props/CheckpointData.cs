using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CheckpointData", menuName = "Scriptable Objects/CheckpointData")]
[Serializable]
public class CheckpointData
{

    [SerializeField]
    private string _checkPointName;
    public string CheckPointName {
        get { return this._checkPointName; }
        set { this._checkPointName = value; }
    }

    [SerializeField]
    private Vector3 _respawnPosition;
    public Vector3 RespawnPosition{
        get { return this._respawnPosition; }
        set { this._respawnPosition = value; }
    }

}
