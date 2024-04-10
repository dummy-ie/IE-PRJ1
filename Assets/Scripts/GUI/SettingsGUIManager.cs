using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SettingsGUIManager : MonoBehaviour
{
    [SerializeField]
    AssetReference _mainMenuSceneReference;

    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _music;
    [SerializeField] private Slider _sfx;

    void Start()
    {
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            _master.value = 1;
            _music.value = 1;
            _sfx.value = 1;
        }
        else
            LoadVolumeSettings();

        LoadVolumeSettings();

        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void LoadVolumeSettings()
    {
        _master.value = AudioManager.Instance.GetMasterVolume();
        _music.value = AudioManager.Instance.GetMusicVolume();
        _sfx.value = AudioManager.Instance.SFXVolume;
    }

    public void ChangeMasterVolume()
    {
        AudioManager.Instance.SetMasterVolume(_master.value);
        AudioManager.Instance.SaveVolumeSettings();
    }

    public void ChangeBGMVolume()
    {
        AudioManager.Instance.SetMusicVolume(_music.value);
        AudioManager.Instance.SaveVolumeSettings();
    }

    public void ChangeSFXVolume()
    {
        AudioManager.Instance.SFXVolume = _sfx.value;
        AudioManager.Instance.SaveVolumeSettings();
    }

    void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadSceneWithFade(_mainMenuSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
    }
}
