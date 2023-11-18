using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckpointData", menuName = "Scriptable Objects/CheckpointData")]
[Serializable]
public class CheckpointData : ScriptableObject
{
    [SerializeField]
    private string baseName;

    [SerializeField]
    private Vector3 basePosition;

    private string _checkPointName;

    public string CheckPointName {
        get { return this._checkPointName; }
        set { this._checkPointName = value; }
    }
    private Vector3 _respawnPosition;

    public Vector3 RespawnPosition{
        get { return this._respawnPosition; }
        set { this._respawnPosition = value; }
    }
    private void OnEnable(){

        this._checkPointName  = this.baseName;
        this._respawnPosition = basePosition;
        
    }

    private void AfterDeserializeField(){

        this._checkPointName  = this.baseName;
        this._respawnPosition = basePosition;
        
    }

}
