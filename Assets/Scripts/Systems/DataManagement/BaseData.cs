using Newtonsoft.Json;
using System;
using UnityEngine;


[Serializable]
[JsonObject]
public class BaseData 
{
    [SerializeField]
    [JsonProperty]
    protected string dataId;
    public string ID
    {
        get { return dataId; }
    }
}
