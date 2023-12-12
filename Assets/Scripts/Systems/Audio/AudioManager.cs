using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    [SerializeField]
    private AudioClip _bgmTheme;

    [SerializeField]
    private List<AudioClip> _sfxClips = new List<AudioClip>();

    public void PlaySFX(EClipIndex index)
    {
        this._sfxSource.clip = this._sfxClips[(int)index];
        if (!this._sfxSource.isPlaying)
            this._sfxSource.PlayOneShot(this._sfxSource.clip, 1);
    }

    public void StopSFX()
    {
        this._sfxSource.Stop();
    }

    public void ChangeBGM(AudioClip bgm) {
        if(bgm != null){
            
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

    private void Start()
    {
        this._bgmSource = this.transform.Find("BGM").GetComponent<AudioSource>();
        this._bgmSource.clip = this._bgmTheme;

        this._sfxSource = this.transform.Find("SFX").GetComponent<AudioSource>();

        this._bgmSource.Play();
    }
}
