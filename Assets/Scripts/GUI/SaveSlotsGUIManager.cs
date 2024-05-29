using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SaveSlotsGUIManager : CanvasMenu, IMenuScreen
{
    [SerializeField]
    AssetReference _gameSceneReference;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private List<Button> _slotButtons = new List<Button>();
    [SerializeField] private List<Button> _clearButtons = new List<Button>();
    [SerializeField] private List<SaveInfoGUI> _saveInfos = new List<SaveInfoGUI>();

    public event System.Action OnMenuDisable;

    // Start is called before the first frame update
    void Start()
    {
        _slotButtons[0].onClick.AddListener(OnSlot1ButtonClicked);
        _slotButtons[1].onClick.AddListener(OnSlot2ButtonClicked);
        _slotButtons[2].onClick.AddListener(OnSlot3ButtonClicked);

        _clearButtons[0].onClick.AddListener(OnClear1ButtonClicked);
        _clearButtons[1].onClick.AddListener(OnClear2ButtonClicked);
        _clearButtons[2].onClick.AddListener(OnClear3ButtonClicked);

        LoadSaves();
    }

    void OnSlot1ButtonClicked()
    {
        Debug.Log("Slot 1 Selected");
        InitializeGame(1);
    }

    void OnSlot2ButtonClicked()
    {
        Debug.Log("Slot 2 Selected");
        InitializeGame(2);
    }

    void OnSlot3ButtonClicked()
    {
        Debug.Log("Slot 3 Selected");
        InitializeGame(3);
    }


    void OnClear1ButtonClicked()
    {
        Debug.Log("Slot 1 Cleared");
        ClearSave(1);
    }

    void OnClear2ButtonClicked()
    {
        Debug.Log("Slot 2 Cleared");
        ClearSave(2);
    }

    void OnClear3ButtonClicked()
    {
        Debug.Log("Slot 3 Cleared");
        ClearSave(3);
    }

    void InitializeGame(int slotNum)
    {
        AssetReference _newGameSceneReference = _gameSceneReference;

        DataRepository tempRepo;

        DataManager.Instance.LoadRepository(slotNum);
        if (DataManager.Instance.CheckSaveExists(slotNum))
            _newGameSceneReference = new AssetReference(DataManager.Instance.Repository.SavedScene);
        DataManager.Instance.UpdateTimeStarted();
        SceneLoader.Instance.LoadSceneWithFade(_newGameSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
    }



    void LoadSaves()
    {
        int i = 1;
        foreach (Button slot in _slotButtons)
        {
            TMP_Text buttonText = slot.gameObject.GetComponentInChildren<TMP_Text>();
            if (DataManager.Instance.CheckSaveExists(i))
            {
                buttonText.text = "Load Save " + i;
                LoadSaveInfo(_saveInfos[i-1]);
            }
            else
            {
                buttonText.text = "Empty";
                _saveInfos[i - 1].DeactivateMenu();
            }
            i++;
        }
    }

    void LoadSaveInfo(SaveInfoGUI saveInfo)
    { 
        DataRepository.SaveInfo repositorySaveInfo = new();

        saveInfo.ActivateMenu();
        saveInfo.HealthText = "Health: " + repositorySaveInfo.playerHealth.ToString();
        saveInfo.ManiteText = "Manite: " + repositorySaveInfo.playerManite.ToString();
        saveInfo.BiomeText = "Biome: " + repositorySaveInfo.biomeText;

        TimeSpan time = TimeSpan.FromSeconds(repositorySaveInfo.timeElapsed);
        string displayTime = time.ToString("hh':'mm':'ss");
        Debug.Log(displayTime);
        saveInfo.TimePlayedText = "Time: " + displayTime;

        saveInfo.LastPlayedText = "Last Played: " + repositorySaveInfo.lastPlayed.ToString("yyyy-MM-dd");
    }

    void ClearSave(int slotNum)
    {
        DataManager.Instance.DeleteSaveFile(slotNum);
        LoadSaves();
    }

    public void ActivateMenu()
    {
        isEnabled = true;
        _canvasGroup.FadeGroup(true, UIUtility.TransitionTime);
    }

    public void DeactivateMenu()
    {
        isEnabled = false;
        _canvasGroup.FadeGroup(false, UIUtility.TransitionTime, () => OnMenuDisable?.Invoke());
    }
}
