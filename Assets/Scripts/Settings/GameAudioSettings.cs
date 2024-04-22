using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameAudioSettings", menuName = "Settings/GameAudioSettings")]
public class GameAudioSettings : ScriptableSingleton<GameAudioSettings>, GameInitializer.IInitializableSingleton
{
    public enum Group
    {
        NONE,
        MASTER,
        MUSIC,
        SFX,
    }

    public string MasterVolumeKey = "masterVolume",
        MusicVolumeKey = "musicVolume",
        SFXVolumeKey = "sfxVolume";

    [Header("Mixers")]
    public AudioMixer AudioMixer;

    [Header("Groups")]
    public AudioMixerGroup MasterGroup;
    public AudioMixerGroup MusicGroup;
    public AudioMixerGroup SFXGroup;

    public void Initialize()
    {
        GameAudioSettings instance = Instance;
        SetVolume(MasterVolumeKey, PlayerPrefs.GetFloat(MasterVolumeKey, 1), false);
        SetVolume(MusicVolumeKey, PlayerPrefs.GetFloat(MusicVolumeKey, 1), false);
        SetVolume(SFXVolumeKey, PlayerPrefs.GetFloat(SFXVolumeKey, 1), false);
    }

    public void SetVolume(string fieldName, float value, bool setPrefs)
    {
        AudioMixer.SetFloat(fieldName, CalculateAudio(value));
        if (setPrefs)
            PlayerPrefs.SetFloat(fieldName, value);
    }

    public void SetMasterVolume(float value) => SetVolume(MasterVolumeKey, value, true);
    public void SetMusicVolume(float value) => SetVolume(MusicVolumeKey, value, true);
    public void SetSFXVolume(float value) => SetVolume(SFXVolumeKey, value, true);

    public static float CalculateAudio(float value)
    {
        if (value == 0)
            return -80;
        return Mathf.Log10(value) * 20;
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterVolumeKey);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MusicVolumeKey);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFXVolumeKey);
}
