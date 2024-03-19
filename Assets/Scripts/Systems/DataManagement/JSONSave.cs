using Newtonsoft.Json;
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
        var info = new DirectoryInfo(this._savePath);
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            Debug.Log(file.Name);
            LoadRepository(file.Name);
        }
        LoadAll();
    }

    private void SetPaths()
    {
        this._savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "/SaveData/";
        this._persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/";
    }

    public void SaveData(BaseData data)
    {
        this._dataRepository.AddData(data);
    }

    public T LoadData<T>(BaseData data)
    {
        if (this._dataRepository == null)
            Debug.LogError("WTF THERES NO DATA REPOSITORY AHAHAHAHAHAH");
        return this._dataRepository.DataList[data.ID];
        //return this._dataRepository.DataList[data.ID] as T;
    }

    public void SaveRepository(BaseData data)
    {
        string savePath = this._savePath + data.ID + ".json";

        Debug.Log("Saving data at " + savePath);
        string json = JsonConvert.SerializeObject(data);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadRepository(string fileName)
    {
        

        string loadPath = this._savePath + fileName;

        if (File.Exists(loadPath))
        {
            using StreamReader reader = new StreamReader(loadPath);
            Debug.Log("Loading data from " + loadPath);

            string json = reader.ReadToEnd();
            reader.Close();
            this._dataRepository.AddData(JsonConvert.DeserializeObject<BaseData>(json));

            
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

        foreach (var data in _dataRepository.DataList)
        {
            SaveRepository(data.Value);
        }
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
