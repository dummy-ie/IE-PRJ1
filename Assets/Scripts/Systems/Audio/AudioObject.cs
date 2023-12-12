using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    private AudioSource _sceneAudio;

    private void OnEnable(){

        _sceneAudio = this.gameObject.GetComponent<AudioSource>();
        AudioClip newClip = this._sceneAudio.clip;
        AudioManager.Instance.ChangeBGM(newClip);
    }
    
}
