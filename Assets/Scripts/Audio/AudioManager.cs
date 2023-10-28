using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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

    public void StopBGM()
    {
        this._bgmSource.Stop();
    }

    /* UNITY LIFECYCLE METHODS */
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        this._bgmSource = this.transform.Find("Background Theme").GetComponent<AudioSource>();
        this._bgmSource.clip = this._bgmTheme;

        this._sfxSource = this.transform.Find("SFX").GetComponent<AudioSource>();

        this._bgmSource.Play();
    }
}
