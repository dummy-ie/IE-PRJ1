using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
[Serializable]
public class InteractableData : BaseData
{

    [SerializeField]
    private bool _enabled;
    public bool Enabled
    {

        get { return this._enabled; }

        set { _enabled = value; }
    }

    InteractableData(string _dataId, bool _enabled)
    {
        this.dataId = _dataId;
        this._enabled = _enabled;
    }
}

