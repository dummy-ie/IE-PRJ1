using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SettingsGUIManager : MonoBehaviour
{
    [SerializeField]
    AssetReference _mainMenuSceneReference;

    [SerializeField]
    AssetReference _rebindSceneReference;

    [SerializeField] private Button _backButton;
    // Temporary
    [SerializeField] private Button _rebindButton;
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _music;
    [SerializeField] private Slider _sfx;

    void Start()
    {
        LoadVolumeSettings();
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    void LoadVolumeSettings()
    {
        _master.value = GameAudioSettings.Instance.GetMasterVolume();
        _music.value = GameAudioSettings.Instance.GetMusicVolume();
        _sfx.value = GameAudioSettings.Instance.GetSFXVolume();
    }

    public void ChangeMasterVolume()
    {
        GameAudioSettings.Instance.SetMasterVolume(_master.value);
    }

    public void ChangeBGMVolume()
    {
        GameAudioSettings.Instance.SetMusicVolume(_music.value);
    }

    public void ChangeSFXVolume()
    {
        GameAudioSettings.Instance.SetSFXVolume(_sfx.value);
    }

    void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadSceneWithFade(_mainMenuSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
    }

    void OnRebindButtonClicked()
    {
        SceneLoader.Instance.LoadSceneWithFade(_rebindSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
    }
}
