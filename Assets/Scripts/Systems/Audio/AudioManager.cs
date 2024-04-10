using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    private float _sfxVolume = 1.0f;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }

    private float _masterVolume = 1.0f;

    public float GetMasterVolume()
    {
        return _masterVolume;
    }
    public void SetMasterVolume(float value)
    {
        _masterVolume = value;
        AudioListener.volume = _masterVolume;
    }

    private float _musicVolume = 1.0f;

    public float GetMusicVolume()
    {
        return _musicVolume;
    }
    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
        _bgmSource.volume = _musicVolume;
    }

    [SerializeField]
    private AudioClip _bgmTheme;

    [SerializeField]
    private List<AudioClip> _sfxClips = new List<AudioClip>();

    public void PlaySFX(EClipIndex index)
    {
        this._sfxSource.clip = this._sfxClips[(int)index];
        if (!this._sfxSource.isPlaying)
            this._sfxSource.PlayOneShot(this._sfxSource.clip, SFXVolume);
    }

    public void StopSFX()
    {
        this._sfxSource.Stop();
    }

    public void ChangeBGM(AudioClip bgm)
    {
        if (bgm != null)
        {

            Debug.Log("Audio clip found of name" + bgm.name);
            if (_bgmSource.clip.name == bgm.name)
                return;

            _bgmSource.Stop();
            _bgmSource.clip = bgm;
            _bgmSource.Play();

        }

        else
            Debug.Log("No audio clip found");

    }

    public void StopBGM()
    {
        this._bgmSource.Stop();
    }

    public void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("masterVolume"));
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        SFXVolume = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("masterVolume", _masterVolume);
        PlayerPrefs.SetFloat("musicVolume", _musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", _sfxVolume);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        this._bgmSource = this.transform.Find("BGM").GetComponent<AudioSource>();
        this._bgmSource.clip = this._bgmTheme;

        this._sfxSource = this.transform.Find("SFX").GetComponent<AudioSource>();

        this._bgmSource.Play();

        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1);
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("sfxVolume", 1);
            LoadVolumeSettings();
        }
        else
            LoadVolumeSettings();

    }
}
