using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDestroyer : MonoBehaviour
{
    private AudioSource _audioSource;

    private float _clipLength;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Start()
    {
        _clipLength = _audioSource.clip.length;

        yield return new WaitForSeconds(_clipLength);

        Destroy(gameObject);
    }
}
