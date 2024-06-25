using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class VideoSettingsGUIManager : MonoBehaviour
{
    [SerializeField]
    AssetReference _mainMenuSceneReference;

    [SerializeField] private Button _backButton;
    
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Toggle _verticalSyncToggle;

    void Awake()
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

    public void OnResolutionChange()
    {
        GameGraphicsSettings.Instance.SetResolution(_resolutionDropdown.value, true);
    }

    public void OnBrightnessChange()
    {
        GameGraphicsSettings.Instance.SetBrightness(_brightnessSlider.value, true);
    }

    public void OnFullScreenChange()
    {
        GameGraphicsSettings.Instance.SetFullScreen(_fullscreenToggle.isOn, true);
    }

    public void OnVerticalSyncChange()
    {
        GameGraphicsSettings.Instance.SetVerticalSync(_verticalSyncToggle.isOn, true);
    }

    public void OnResetDefaultsClicked()
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
}
