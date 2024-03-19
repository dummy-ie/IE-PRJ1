using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JSONSave : Singleton<JSONSave>
{

    private string _savePath;
    private string _persistentPath;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public T LoadData<T>(BaseData data) where T : class
    {
        if (this._dataRepository == null)
            Debug.LogError("WTF THERES NO DATA REPOSITORY AHAHAHAHAHAH");
        return this._dataRepository.RetrieveData(data) as T;
    }

    /*public void SaveData(BaseData data)
    {
        string savePath = this._savePath + data.ID + ".json";

        Debug.Log("Saving data at " + savePath);
        string json = JsonUtility.ToJson(data);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData(BaseData data)
    {
        string loadPath = this._savePath + data.ID + ".json";

        using StreamReader reader = new StreamReader(loadPath);
        Debug.Log("Loading data from " + loadPath);

        string json = reader.ReadToEnd();
        reader.Close();
        JsonUtility.FromJsonOverwrite(json, data);

    }*/

    public void SaveAll()
    {
        Debug.Log("Saving All Data");
        ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        foreach (var saver in saveables)
        {
            saver.SaveData();
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
