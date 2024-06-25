using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(menuName = "Scriptable Objects/Scene Management/Scene Data")]
public class SceneData : ScriptableObject
{
    public enum SceneType
    {
        GAMEPLAY,
        MAIN_MENU,
        GAME_OVER
    }

    public AudioObject sceneBGM;

    public SceneType sceneType;

    public AssetReference SceneReference;
    public SpawnPoints spawnPoints;

    public AsyncOperationHandle<SceneInstance> Operation;
}
