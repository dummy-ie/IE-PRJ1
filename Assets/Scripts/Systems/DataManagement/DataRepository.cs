using Ink.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[JsonObject]
public class DataRepository
{
    [JsonProperty]
    private Dictionary<string, dynamic> _dataList = new();

    [JsonIgnore]
    public Dictionary<string, dynamic> DataList
    {
        get { return _dataList; }
        set { _dataList = value; }
    }

    public void AddData<T>(T newData) where T : BaseData
    {
        Debug.Log("Adding Data ["+ newData.ID + "]...");
        if (!_dataList.ContainsKey(newData.ID))
        {
            _dataList.Add(newData.ID, newData);
        }
        else
        {
            Debug.Log("Existing key already exists!");
        }
    }

    public void RetrieveData<T>(ref T data) where T : BaseData
    {
        if (_dataList.ContainsKey(data.ID))
        {

            Debug.Log("["+data.ID+"] key found! Retrieving...");

            if (_dataList[data.ID] is JObject)
            {
                Debug.Log(_dataList[data.ID]);
                T obj = JsonConvert.DeserializeObject<T>(_dataList[data.ID]);
                Debug.Log(obj);
                //data = obj;
            }
            else
            {
                data = _dataList[data.ID];
            }

            
        } 
        else
        {
            Debug.Log("Data ID [" + data.ID + "] does not exist!");
            Debug.Log("Generating new data for ["+ data.ID +"]");
            AddData(data);
            data = _dataList[data.ID];
        }
    }

   
}
