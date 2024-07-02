using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] private AssetReference _mobSceneReference;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            SceneLoader.Instance.LoadSceneWithFade(_mobSceneReference, new SceneLoader.TransitionData());
    }
}
