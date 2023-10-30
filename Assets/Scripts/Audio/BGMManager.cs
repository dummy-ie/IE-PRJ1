using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager> {
    [SerializeField]    
    private List<AudioClip> _clips;
    private AudioSource _source;

    public AudioClip GetAudio(string name) { 
        foreach (AudioClip clip in _clips) {
            if (clip.name == name) {
                return clip;
            }
        }
        return null;
    }

    public void Play(string name, bool loop = false) { 
        foreach (AudioClip clip in _clips) {
            if (clip.name == name) {
                this._source.clip = clip;
                this._source.loop = loop;
                this._source.Play();
                return;
            }
        }
    }

    public void Stop() { 
        this._source.Stop();
        this._source.clip = null;
    }

    void Start() {
        
    }
}
