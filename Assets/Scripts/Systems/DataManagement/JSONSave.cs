using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JSONSave : Singleton<JSONSave>
{

    private string _savePath;
    private string _persistentPath;

    

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

    public void SaveData(InteractableData interactData)
    {
        string savePath = this._savePath + interactData.ObjectName + ".json";

        Debug.Log("Saving data at " + savePath);
        string json = JsonUtility.ToJson(interactData);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData(InteractableData interactData)
    {
        StartCoroutine(BufferLoadData(interactData));
    }

    private IEnumerator BufferLoadData(InteractableData interactData)
    {
        yield return new WaitForSeconds(.1f);

        string loadPath = this._savePath + interactData.ObjectName + ".json";

        using StreamReader reader = new StreamReader(loadPath);
        Debug.Log("Loading data from " + loadPath);

        string json = reader.ReadToEnd();
        reader.Close();
        interactData = JsonUtility.FromJson<InteractableData>(json);
    }

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
