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

    public T RetrieveData<T>(T getData) where T : BaseData
    {
        if (_dataList.ContainsKey(getData.ID))
        {
            return _dataList[getData.ID] as T;
        }
        else
        {
            Debug.Log("Data ID [" + getData.ID + "] does not exist!");
            Debug.Log("Generating new data for [" + getData.ID + "]");
            AddData(getData);
            return _dataList[getData.ID] as T;
        }
    }
}
