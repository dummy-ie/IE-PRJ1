using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{

    public struct TransitionData
    {
        public string spawnPoint;
        public SceneData currentScene;
    }

    [SerializeField]
    private AssetReference _mainMenuReference;
    private string _sceneName;

    private GameObject[] _spawnPoints;
    private GameObject[] sceneConnections;

    private SceneData _activeScene;
    public SceneData ActiveScene
    {
        get { return _activeScene; }
    }

    private AsyncOperationHandle<SceneData> _sceneDataAssetHandle;

    private AssetReference _activeSceneReference;
    public AssetReference ActiveSceneReference
    {
        get { return _activeSceneReference; }
    }
    public void LoadMainMenu()
    {
        LoadSceneWithoutFade(_mainMenuReference, new TransitionData());
    }
    public void LoadSceneWithoutFade(string sceneName)
    {
        _sceneName = sceneName;
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadSceneWithoutFade(AssetReference sceneData, TransitionData transitionData)
    {
        /*_activeSceneReference = sceneData;
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded) 
                StartCoroutine(SceneLoad((SceneData)sceneData.Asset, transitionData));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };*/
        _activeSceneReference = sceneData;
        
        StartCoroutine(SceneLoad(LoadScene(sceneData), transitionData));
    }
    public void LoadSceneWithFade(AssetReference sceneData, TransitionData transitionData)
    {
        /*_activeSceneReference = sceneData;
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                StartCoroutine(SceneLoadWithFade((SceneData)sceneData.Asset, transitionData));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };*/
        _activeSceneReference = sceneData;
        StartCoroutine(SceneLoadWithFade(LoadScene(sceneData), transitionData));
    }
    private IEnumerator FadeAndLoadScene()
    {
        yield return ScreenFader.Instance.FadeOut();
        SceneManager.LoadScene(_sceneName);
        yield return ScreenFader.Instance.FadeIn();
    }

    private IEnumerator FadeAndLoadAsyncScene()
    {
        yield return ScreenFader.Instance.FadeOut();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log("grr");
            yield return null;
        }
        yield return ScreenFader.Instance.FadeIn();
    }

    public void LoadScene(string sceneName, bool async = false)
    {
        _sceneName = sceneName;
        if (async)
            StartCoroutine(FadeAndLoadAsyncScene());
        else
            StartCoroutine(FadeAndLoadScene());
    }
    private IEnumerator SceneLoad(SceneData sceneData, TransitionData transitionData)
    {
        Debug.Log("Loading Scene...");

        if (_sceneDataAssetHandle.IsValid())
            Addressables.ReleaseInstance(_sceneDataAssetHandle);

        _activeScene = sceneData;
        transitionData.currentScene = _activeScene;

        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;
        while (!handle.IsDone)
        {
            yield return null;
        }

        OnSceneLoad(transitionData);
    }

    private IEnumerator SceneLoadWithFade(SceneData sceneData, TransitionData transitionData)
    {
        Debug.Log("Loading Scene...");

        if (_sceneDataAssetHandle.IsValid())
            Addressables.ReleaseInstance(_sceneDataAssetHandle);

        _activeScene = sceneData;
        transitionData.currentScene = _activeScene;

        //yield return ScreenFader.Instance.FadeOut();
        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;

        while (!handle.IsDone)
        {
            yield return null;
        }

        OnSceneLoad(transitionData);
    }

    private SceneData LoadScene(AssetReference sceneReference)
    {
        _sceneDataAssetHandle = sceneReference.LoadAssetAsync<SceneData>();
        return _sceneDataAssetHandle.WaitForCompletion();
    }

    private void OnSceneLoad(TransitionData transitionData)
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController2D>().OnSceneLoad(transitionData);
        }
    }
}
