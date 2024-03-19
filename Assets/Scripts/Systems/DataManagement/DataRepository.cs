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
    private SerializableDictionary<string, BaseData> _dataList = new();
    public SerializableDictionary<string, BaseData> DataList
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

    public T RetrieveData<T>(string ID)
    {
        if (_dataList.ContainsKey(ID))
        {

            return  (T)_dataList[ID];
        } 
        else
        {
            Debug.Log("Data ID [" + ID + "] does not exist!");
            return default;
        }
    }
}
