using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlatformEffector2D))]
public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D _platform;

    void Start()
    {
        _platform = GetComponent<PlatformEffector2D>();
    }

    public void StartDropPlatform()
    {
        StartCoroutine(DropPlatform());
    }

    public IEnumerator DropPlatform()
    {
        _platform.surfaceArc = 0f;
        yield return new WaitForSeconds(0.5f);
        _platform.surfaceArc = 180f;
    }
}

