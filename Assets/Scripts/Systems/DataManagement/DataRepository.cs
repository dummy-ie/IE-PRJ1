using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[JsonObject]
public class DataRepository
{
    [SerializeField]
    [JsonProperty]
    private Dictionary<string, BaseData> _dataList = new();

    public Dictionary<string, BaseData> DataList
    {
        get { return _dataList; }
    }

    public void AddData(BaseData newData)
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

    public T RetrieveData<T>(BaseData data) where T : class
    {
        if (_dataList.ContainsKey(data.ID))
        {
            

            Debug.Log("["+data.ID+"] key found! Retrieving...");
            return _dataList[data.ID] as T;
            
        } 
        else
        {
            Debug.Log("Data ID [" + data.ID + "] does not exist!");
            Debug.Log("Generating new data for ["+ data.ID +"]");
            AddData(data);
            return default;
        }
    }

   
}
