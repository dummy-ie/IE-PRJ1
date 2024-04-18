using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, IOnSceneLoad
{
    public AudioSource _bgmSource;
    public AudioSource SFXSource;

    private float _sfxVolume = 1.0f;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }

    private float _masterVolume = 1.0f;

    [SerializeField]
    private AudioObject _bgmTheme;

    [SerializeField]
    private List<AudioObject> _sfxObjects = new List<AudioObject>();

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

    public void PlaySFX(EClipIndex index, Vector3 position)
    {
        AudioSource source = Instantiate(SFXSource, position, Quaternion.identity);
        this._sfxObjects[(int)index].Clone(source);
        source.Play();
    }

    //public void StopSFX()
    //{
    //    this._sfxSource.Stop();
    //}

    public void ChangeBGM(AudioObject bgm)
    {
        if (bgm.clip != null)
        {
            Debug.Log("Audio clip found of name" + bgm.clip.name);
            if (_bgmSource.clip.name == bgm.clip.name)
                return;
            Debug.Log("Playing new BGM");
            _bgmSource.Stop();
            bgm.Clone(_bgmSource);
            _bgmSource.Play();
        }
        else
            Debug.Log("No audio clip found");

    }

    public void StopBGM() {
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
        this._bgmSource = transform.Find("BGM").GetComponent<AudioSource>();
        //this._bgmTheme.Clone(this._bgmSource);
        if (_bgmTheme != null)
            this._bgmTheme.Clone(this._bgmSource);
        //this._sfxSource = this.transform.Find("SFX").GetComponent<AudioSource>();

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

    public void OnSceneLoad(SceneLoader.TransitionData transitionData)
    {
        ChangeBGM(transitionData.currentScene.sceneBGM);
    }
}
