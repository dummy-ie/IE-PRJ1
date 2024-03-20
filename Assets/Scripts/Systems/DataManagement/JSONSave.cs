using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JSONSave : Singleton<JSONSave>
{

    private string _savePath;
    private string _persistentPath;

    [JsonProperty]
    [SerializeField]
    private DataRepository _dataRepository = new();
    public DataRepository Repository { get { return _dataRepository; } }

    // Start is called before the first frame update
    void Start()
    {
        SetPaths();

        /*try
        {
            if (!Directory.Exists(_persistentPath))
            {
                Directory.CreateDirectory(_persistentPath);
            }

        }
        catch (IOException ex)
        {
        }*/
        LoadRepository();
    }

    private void SetPaths()
    {
        this._savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData/";
        this._persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/";
    }

    public void SaveData(BaseData data)
    {
        this._dataRepository.AddData(data);
    }

    public T LoadData<T>(BaseData data) where T : class
    {
        Debug.Log("LoadData called for ["+ data.ID +"]");
        return this._dataRepository.RetrieveData<T>(data);
        //return this._dataRepository.DataList[data.ID] as T;
    }

    public void SaveRepository()
    {
        string savePath = this._savePath + "SaveData.json";

        Debug.Log("Saving data at " + savePath);
        string json = JsonConvert.SerializeObject(this._dataRepository);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadRepository()
    {
        

        string loadPath = this._savePath + "SaveData.json";

        if (File.Exists(loadPath))
        {
            using StreamReader reader = new StreamReader(loadPath);
            Debug.Log("Loading data from " + loadPath);

            string json = reader.ReadToEnd();
            reader.Close();

            Debug.Log(json);
            
            this._dataRepository = JsonConvert.DeserializeObject<DataRepository>(json);
            Debug.Log("dataList Size:" + _dataRepository.DataList.Count);

        }
        else
        {
            Debug.Log("Save Data does not exist");
        }

        //this._dataRepository.RefreshData();
    }

    public void SaveAll()
    {
        Debug.Log("Saving All Data");
        /*ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        foreach (var saver in saveables)
        {
            saver.SaveData();
        }*/

        SaveRepository();

    }

    public void LoadAll()
    {
        Debug.Log("Loading All Data");
        ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        foreach (var saver in saveables)
        {
            saver.LoadData();
        }
    }
}
