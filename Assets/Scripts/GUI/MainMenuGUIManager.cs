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

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _loadButton.onClick.AddListener(OnLoadButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        DataManager.Instance.LoadRepository(1);
        SceneLoader.Instance.LoadSceneWithFade(_gameSceneReference, new SceneLoader.TransitionData {spawnPoint = "default"});
    }

    void OnLoadButtonClicked()
    {
        DataManager.Instance.LoadRepository(1);
        SceneLoader.Instance.LoadSceneWithFade(new AssetReference(DataManager.Instance.Repository.SavedScene), new SceneLoader.TransitionData { spawnPoint = "default" });
    }

    void OnSettingsButtonClicked()
    {
        SceneLoader.Instance.LoadSceneWithFade(_settingsSceneReference, new SceneLoader.TransitionData {spawnPoint = "default"});

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
