using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class OptionsGUIManager : CanvasMenu, IMenuScreen
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Panels")]
    [SerializeField] private CanvasGroup _videoGroup;
    [SerializeField] private CanvasGroup _audioGroup;
    [SerializeField] private CanvasGroup _keyboardGroup;
    [SerializeField] private CanvasGroup _gamepadGroup;

    [Header("Panel Toggle")]
    [SerializeField] private Toggle _videoToggle;
    [SerializeField] private Toggle _audioToggle;
    [SerializeField] private Toggle _keyboardToggle;
    [SerializeField] private Toggle _gamepadToggle;

    [Header("Video Settings")]
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Toggle _verticalSyncToggle;
    [SerializeField] private Button _resetVideoSettingsButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _music;
    [SerializeField] private Slider _sfx;

    public event System.Action OnMenuDisable;

    void Awake()
    {
        LoadVideoSettings();
        LoadVolumeSettings();

        _videoToggle.onValueChanged.AddListener(ToggleVideo);
        _audioToggle.onValueChanged.AddListener(ToggleAudio);
        _keyboardToggle.onValueChanged.AddListener(ToggleKeyboard);
        _gamepadToggle.onValueChanged.AddListener(ToggleGamepad);
    }

    void LoadVideoSettings()
    {
        _brightnessSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GameGraphicsSettings.Instance.brightnessPrefsKey, GameGraphicsSettings.Instance.defaultBrightnessValue));

        List<TMP_Dropdown.OptionData> options = new();
        foreach (Vector2Int resolution in GameGraphicsSettings.Instance.screenResolutions)
        {
            options.Add(new TMP_Dropdown.OptionData($"{resolution.x}x{resolution.y}"));
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.resolutionPrefsKey, GameGraphicsSettings.Instance.defaultResolutionIndex));

        _fullscreenToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.fullScreenPrefsKey, GameGraphicsSettings.Instance.defaultFullScreenValue ? 1 : 0)));
        _verticalSyncToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.verticalSyncPrefsKey, GameGraphicsSettings.Instance.defaultVerticalSyncValue ? 1 : 0)));
    }

    void LoadVolumeSettings()
    {
        _master.value = GameAudioSettings.Instance.GetMasterVolume();
        _music.value = GameAudioSettings.Instance.GetMusicVolume();
        _sfx.value = GameAudioSettings.Instance.GetSFXVolume();
    }
    void OnVideoResetDefaultsClicked()
    {
        GameGraphicsSettings.Instance.SetResolution(GameGraphicsSettings.Instance.defaultResolutionIndex, true);
        GameGraphicsSettings.Instance.SetBrightness(GameGraphicsSettings.Instance.defaultBrightnessValue, true);
        GameGraphicsSettings.Instance.SetFullScreen(GameGraphicsSettings.Instance.defaultFullScreenValue, true);
        GameGraphicsSettings.Instance.SetVerticalSync(GameGraphicsSettings.Instance.defaultVerticalSyncValue, true);

        _resolutionDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.resolutionPrefsKey, GameGraphicsSettings.Instance.defaultResolutionIndex));
        _brightnessSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GameGraphicsSettings.Instance.brightnessPrefsKey, GameGraphicsSettings.Instance.defaultBrightnessValue));
        _fullscreenToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.fullScreenPrefsKey, GameGraphicsSettings.Instance.defaultFullScreenValue ? 1 : 0)));
        _verticalSyncToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt(GameGraphicsSettings.Instance.verticalSyncPrefsKey, GameGraphicsSettings.Instance.defaultVerticalSyncValue ? 1 : 0)));
    }
    public Tween FadeGroup(bool enabled, CanvasGroup group)
    {
        return group.FadeGroup(enabled, UIUtility.TransitionTime, () => {
            if (enabled)
            {
                group.blocksRaycasts = enabled;
                group.interactable = enabled;
            }
        });
    }

    private void ToggleVideo(bool enabled) => FadeGroup(enabled, _videoGroup);
    private void ToggleAudio(bool enabled) => FadeGroup(enabled, _audioGroup);
    private void ToggleKeyboard(bool enabled) => FadeGroup(enabled, _keyboardGroup);
    private void ToggleGamepad(bool enabled) => FadeGroup(enabled, _gamepadGroup);

    public void OnResolutionChange() => GameGraphicsSettings.Instance.SetResolution(_resolutionDropdown.value, true);
    public void OnBrightnessChange() => GameGraphicsSettings.Instance.SetBrightness(_brightnessSlider.value, true);
    public void OnFullScreenChange() => GameGraphicsSettings.Instance.SetFullScreen(_fullscreenToggle.isOn, true);
    public void OnVerticalSyncChange() => GameGraphicsSettings.Instance.SetVerticalSync(_verticalSyncToggle.isOn, true);
    public void ChangeMasterVolume() => GameAudioSettings.Instance.SetMasterVolume(_master.value);
    public void ChangeBGMVolume() => GameAudioSettings.Instance.SetMusicVolume(_music.value);
    public void ChangeSFXVolume() => GameAudioSettings.Instance.SetSFXVolume(_sfx.value);
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
