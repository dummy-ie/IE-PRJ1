using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Object", menuName = "Scriptable Objects/AudioObject")]
public class AudioObject : ScriptableObject
{
    public AudioClip clip;

    public bool loop;
    [Range(0, 256)] public int priority = 128;
    [Range(0, 1)] public float volume = 0.4f;
    [Range(0, 2)] public float pitch = 1;
    [Range(0, 1)] public float spatialBlend = 1;
    [Header("3D Sound Settings")]
    [Space] public float minDistance = 1;
    public float maxDistance = 500;

    public void Clone(AudioSource source)
    {
        if (!source)
            throw new System.ArgumentNullException(nameof(source));
        source.clip = clip;
        //source.outputAudioMixerGroup = GameAudioSettings.instance.GetGroup(group);
        source.loop = loop;
        source.priority = priority;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
    }


    /*private AudioSource _sceneAudio;

    private void OnEnable(){

        _sceneAudio = this.gameObject.GetComponent<AudioSource>();
        AudioClip newClip = this._sceneAudio.clip;
        AudioManager.Instance.ChangeBGM(newClip);
    }*/

}
