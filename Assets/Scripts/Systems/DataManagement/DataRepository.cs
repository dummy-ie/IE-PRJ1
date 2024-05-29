using Ink.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;

[Serializable]
[JsonObject]
public class DataRepository
{
    [JsonIgnore]
    private Dictionary<string, BaseData> _dataList = new();
    [JsonIgnore]
    public Dictionary<string, BaseData> DataList
    {
        get { return _dataList; }
    }

    [JsonProperty]
    private Dictionary<string, string> _jsonList = new();

    [SerializeField]
    List<string> jsonStrings = new List<string>();

    [JsonProperty]
    private string _savedSceneGUID;
    [JsonIgnore]
    public string SavedScene
    {
        get { return _savedSceneGUID; }
        set { _savedSceneGUID = value; }
    }
 
    public struct SaveInfo
    {
        public int playerHealth;
        public int playerManite;
        public string biomeText;
        public DateTime lastPlayed;
        public double timeElapsed;
    }

    [JsonProperty]
    public SaveInfo _saveInfo;

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
            Debug.Log("[" + data.ID + "] key found in dataList! Retrieving...");

            T cast = (T)_dataList[data.ID];
            data = cast;
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
