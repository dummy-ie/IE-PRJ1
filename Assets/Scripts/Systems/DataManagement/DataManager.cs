using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.PlayerLoop;
using UnityEngine.ResourceManagement.Util;

[CreateAssetMenu(fileName = "DataManager", menuName = "Scriptable Singletons/DataManager")]
public class DataManager : ScriptableSingleton<DataManager>, GameInitializer.IInitializableSingleton
{
    private int _selectedSave = -1;    

    private string _savePath;
    private string _persistentPath;

    [JsonProperty]
    [SerializeField]
    private DataRepository _dataRepository;
    public DataRepository Repository { get { return _dataRepository; } }

    List<BaseData> datas = new List<BaseData>();

    private DateTime _timeStarted;

    // Start is called before the first frame update
    public void Initialize()
    {
        DataManager instance = Instance;
        SetPaths();

        //LoadRepository(1);
    }

    private void SetPaths()
    {
        this._savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData/";
        this._persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/";
    }

    public void SaveData<T>(T data) where T : BaseData
    {
        this._dataRepository.AddData(data);
    }

    public void LoadData<T>(ref T data) where T : BaseData
    {
        Debug.Log("LoadData called for ["+ data.ID +"]");
        this._dataRepository.RetrieveData(ref data);
        //return this._dataRepository.DataList[data.ID] as T;
    }



    public void SaveRepository()
    {
        string savePath = this._savePath + "SaveData_" + this._selectedSave + ".json";

        Debug.Log("Saving data at " + savePath);

        _dataRepository.ConvertToJson();

        string json = JsonConvert.SerializeObject(this._dataRepository);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadRepository(int slotNum)
    {
        Debug.Log("--LOADING REPOSITORY--");

        if (CheckExistingSave(slotNum))
        {
            this._dataRepository = RetrieveRepository(slotNum);
        }
        else
        {
            Debug.Log("Save File " + this._selectedSave + " does not exist");
            NewRepository();
        }

        SetSelectedSave(slotNum);

        //this._dataRepository.RefreshData();
    }

    public DataRepository RetrieveRepository(int slotNum)
    {
        string loadPath = this._savePath + "SaveData_" + slotNum + ".json";

        using StreamReader reader = new StreamReader(loadPath);
        Debug.Log("Loading data from " + loadPath);

        string json = reader.ReadToEnd();
        reader.Close();

        Debug.Log(json);

        return JsonConvert.DeserializeObject<DataRepository>(json);
    }


    public void SetSelectedSave(int selectedSave)
    {
        this._selectedSave = selectedSave;
    }

    public bool CheckExistingSave(int slotNum)
    {
        Debug.Log("--CHECKING FILE--");
        Debug.Log("save path ->" + this._savePath);
        string loadPath = this._savePath + "SaveData_" + slotNum + ".json";
        Debug.Log(loadPath);

        return File.Exists(loadPath);
    }

    public void DeleteSaveFile(int slotNum)
    {
        Debug.Log("--DELETING FILE--");

        string path = this._savePath + "SaveData_" + slotNum + ".json";

        //FileUtil.DeleteFileOrDirectory(path);
        //AssetDatabase.Refresh();
    }

    public void NewRepository()
    {
        Debug.Log("--CREATING NEW REPOSITORY--");

        this._dataRepository = new();
    }

    public void SaveAll()
    {
        Debug.Log("Saving All Data");
        ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        foreach (var saver in saveables)
        {
            saver.SaveData();
        }

        this._dataRepository.SavedScene = SceneLoader.Instance.ActiveSceneReference.AssetGUID;

        UpdateSaveInfo();
        UpdateTimeStarted();

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

    private void UpdateSaveInfo()
    {
        PlayerSavableData playerData = (PlayerSavableData)this._dataRepository.DataList["PlayerStats"];
        this._dataRepository._saveInfo.playerHealth = playerData.Health;
        this._dataRepository._saveInfo.playerManite = playerData.Manite;
        this._dataRepository._saveInfo.biomeText = "Biome";
        this._dataRepository._saveInfo.timeElapsed += DateTime.Now.Subtract(this._timeStarted).TotalSeconds;
        this._dataRepository._saveInfo.lastPlayed = DateTime.Today;
    }

    public void UpdateTimeStarted()
    {
        this._timeStarted = DateTime.Now;
    }
}
