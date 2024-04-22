using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, IOnSceneLoad
{
    public AudioSource _bgmSource;
    public AudioSource SFXSource;

    [SerializeField]
    private AudioObject _bgmTheme;

    [SerializeField]
    private List<AudioObject> _sfxObjects = new List<AudioObject>();

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

    private void Start()
    {
        this._bgmSource = transform.Find("BGM").GetComponent<AudioSource>();
        //this._bgmTheme.Clone(this._bgmSource);
        if (_bgmTheme != null)
            this._bgmTheme.Clone(this._bgmSource);
        //this._sfxSource = this.transform.Find("SFX").GetComponent<AudioSource>();

        this._bgmSource.Play();
    }

    public void OnSceneLoad(SceneLoader.TransitionData transitionData)
    {
        ChangeBGM(transitionData.currentScene.sceneBGM);
    }
}
