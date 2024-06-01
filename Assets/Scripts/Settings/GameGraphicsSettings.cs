using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameGraphicsSettings", menuName = "Settings/GameGraphicsSettings")]
public class GameGraphicsSettings : ScriptableSingleton<GameGraphicsSettings>, GameInitializer.IInitializableSingleton
{
    [Header("Resolution")] public string resolutionPrefsKey = "selectedResolution";
    public int defaultResolutionIndex;
    public List<Vector2Int> screenResolutions;

    [Header("Brightness")] public string brightnessPrefsKey = "selectedBrightness";
    public float defaultBrightnessValue;

    [Header("Full Screen")] public string fullScreenPrefsKey = "isFullScreen";
    public bool defaultFullScreenValue;

    [Header("Vertical Sync")] public string verticalSyncPrefsKey = "isVerticalSync";
    public bool defaultVerticalSyncValue;
    public void Initialize()
    {
        GameGraphicsSettings instance = Instance;
        
        SetResolution(PlayerPrefs.GetInt(resolutionPrefsKey, defaultResolutionIndex), false);
        SetBrightness(PlayerPrefs.GetFloat(brightnessPrefsKey, defaultBrightnessValue), false);
        SetFullScreen(Convert.ToBoolean(PlayerPrefs.GetInt(fullScreenPrefsKey, defaultFullScreenValue ? 1 : 0)), false);
        SetVerticalSync(Convert.ToBoolean(PlayerPrefs.GetInt(verticalSyncPrefsKey, defaultVerticalSyncValue ? 1 : 0)), false);
    }

    public void SetResolution(int index, bool setPrefs)
    {
        index = Mathf.Clamp(index, 0, screenResolutions.Count);
        Vector2Int resolution = screenResolutions[index];
        Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
        if (setPrefs)
            PlayerPrefs.SetInt(resolutionPrefsKey, index);
        //changeResolutionChannel?.Raise(idx);
    }

    public void SetBrightness(float value, bool setPrefs)
    {
        // TODO : ADD POST PROCESSING GLOBAL VOLUME FOR BRIGHTNESS
        if (setPrefs)
            PlayerPrefs.SetFloat(brightnessPrefsKey, value);
    }

    /*public void SetQuality(int idx, bool setPrefs)
    {
        idx = Mathf.Clamp(idx, 0, qualitiesNames.Length);
        QualitySettings.SetQualityLevel(idx);
        if (setPrefs)
            PlayerPrefs.SetInt(qualityPrefsKey, idx);
        changeQualityChannel?.Raise(idx);
    }*/

    public void SetFullScreen(bool isFullScreen, bool setPrefs)
    {
        Screen.fullScreen = isFullScreen;
        if (setPrefs)
            PlayerPrefs.SetInt(fullScreenPrefsKey, isFullScreen ? 1 : 0);
        //changeFullScreenChannel?.Raise(isFullScreen);
    }

    public void SetVerticalSync(bool isVerticalSync, bool setPrefs)
    {
        // TODO : ADD VERTICAL SYNC
        if (setPrefs)
            PlayerPrefs.SetInt(verticalSyncPrefsKey, isVerticalSync ? 1 : 0);
        //changeFullScreenChannel?.Raise(isFullScreen);
    }

    private void OnEnable()
    {
        Debug.Log(Screen.resolutions);
        for (int i = Screen.resolutions.Length - 1; i >= 0; i--)
        {
            screenResolutions.Add(new Vector2Int(Screen.resolutions[i].width, Screen.resolutions[i].height)); 
        }
    }

    private void OnDisable()
    {
        screenResolutions.Clear();
    }
}
