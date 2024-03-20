using Newtonsoft.Json;
using System;
using UnityEngine;


[Serializable]
public class BaseData 
{
    [SerializeField]
    protected string _dataId;
    [JsonProperty]
    public string ID
    {
        get { return _dataId; }
        set { _dataId = value; }
    }
}
