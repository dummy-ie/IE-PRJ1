using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    private AudioSource _sceneAudio;

    private void OnEnable(){

        _sceneAudio = this.gameObject.GetComponent<AudioSource>();
        AudioClip newBGM = this._sceneAudio.clip;
        AudioManager.Instance.ChangeBGM(newBGM);

    }
    
}
