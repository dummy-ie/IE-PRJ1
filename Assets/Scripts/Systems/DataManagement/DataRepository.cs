using Ink.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[JsonObject]
public class DataRepository
{
    [SerializeField]
    [JsonIgnore]
    private SerializableDictionary<string, BaseData> _dataList = new();

    [JsonProperty]
    private Dictionary<string, string> _jsonList = new();

    [JsonIgnore]
    public Dictionary<string, string> JsonList
    {
        get { return _jsonList; }
        set { _jsonList = value; }
    }

    [SerializeField]
    List<string> jsonStrings = new List<string>();

    

    public void AddData<T>(T newData) where T : BaseData
    {
        Debug.Log("Adding Data ["+ newData.ID + "]...");
        if (!_dataList.ContainsKey(newData.ID))
        {
            _dataList.Add(newData.ID, newData);
        }
        else
        {
            Debug.Log("Existing key already exists! Replacing");
        }
    }

    public void RetrieveData<T>(ref T data) where T : BaseData
    {
        if (_dataList.ContainsKey(data.ID))
        {

            Debug.Log("[" + data.ID + "] key found in dataList! Retrieving...");

            string json = JsonUtility.ToJson(_dataList[data.ID]);

            Debug.Log(json);

            data = JsonUtility.FromJson<T>(json);



        }
        else if (_jsonList.ContainsKey(data.ID))
        {
            Debug.Log("[" + data.ID + "] key found in jsonList! Retrieving...");
            data = JsonUtility.FromJson<T>(_jsonList[data.ID]);

            AddData(data);

        }
        else
        {
            Debug.Log("Data ID [" + data.ID + "] does not exist! Generating new data for ["+ data.ID +"]");
            AddData(data);

            string json = JsonUtility.ToJson(_dataList[data.ID]);

            data = JsonUtility.FromJson<T>(json);
        }
    }

    public void ConvertToJson()
    {
        _jsonList.Clear();
        foreach (var item in _dataList)
        {
            string json = JsonUtility.ToJson(item.Value);

            _jsonList.Add(item.Key, json);
        }
    }
}
