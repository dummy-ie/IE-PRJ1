using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[JsonObject]
public class InteractableData : BaseData
{
    [SerializeField]
    private bool _enabled;
    [JsonProperty]
    public bool Enabled
    {

        get { return this._enabled; }

        set { _enabled = value; }
    }

    [JsonConstructor]
    InteractableData(string _dataId, bool _enabled)
    {
        this._dataId = _dataId;
        this._enabled = _enabled;
    }
}

