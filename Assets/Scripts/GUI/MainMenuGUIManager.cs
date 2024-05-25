using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainMenuGUIManager : MonoBehaviour
{
    [SerializeField]
    AssetReference _gameSceneReference;
    
    [SerializeField]
    AssetReference _settingsSceneReference;

    [SerializeField] private Canvas _mainButtons;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _extrasButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private Canvas _saveSlots;

    [SerializeField] private Button _slot1Button;
    [SerializeField] private Button _slot2Button;
    [SerializeField] private Button _slot3Button;
    [SerializeField] private Button _backButton;

    private bool isMakingNewGame = false;

    void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _loadButton.onClick.AddListener(OnLoadButtonClicked);
        _extrasButton.onClick.AddListener(OnExtrasButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);

        _slot1Button.onClick.AddListener(OnSlot1ButtonClicked);
        _slot2Button.onClick.AddListener(OnSlot2ButtonClicked);
        _slot3Button.onClick.AddListener(OnSlot3ButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);

        _saveSlots.enabled = false;
    }

    void OnPlayButtonClicked()
    {
        isMakingNewGame = true;
        ToggleSaves();
    }

    void OnLoadButtonClicked()
    {
        isMakingNewGame = false;
        ToggleSaves();
        
    }

    void OnExtrasButtonClicked(){
        
    }

    void OnSettingsButtonClicked()
    {
        SceneLoader.Instance.LoadSceneWithFade(_settingsSceneReference, new SceneLoader.TransitionData {spawnPoint = "default"});

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

    void OnBackButtonClicked()
    {
        ToggleSaves();
    }

    void InitializeGame(int slotNum)
    {
        if (isMakingNewGame)
        {
            DataManager.Instance.SetSelectedSave(slotNum);
            DataManager.Instance.NewRepository();
            SceneLoader.Instance.LoadSceneWithFade(_gameSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
        }
        else
        {
            DataManager.Instance.SetSelectedSave(slotNum);
            DataManager.Instance.LoadRepository();
            SceneLoader.Instance.LoadSceneWithFade(new AssetReference(DataManager.Instance.Repository.SavedScene), new SceneLoader.TransitionData { spawnPoint = "default" });
        }
    }

    void ToggleSaves()
    {
        _saveSlots.enabled = !_saveSlots.enabled;
        _mainButtons.enabled = !_mainButtons.enabled;
    }

    void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
