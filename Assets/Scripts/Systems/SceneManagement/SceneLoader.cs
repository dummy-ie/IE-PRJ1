using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
    [SerializeField]
    private AssetReference _mainMenuReference;
    private string _sceneName;
    private SceneData _activeScene;
    private GameObject[] sceneConnections;
    public SceneData ActiveScene
    {
        get { return _activeScene; }
    }
    private AssetReference _activeSceneReference;
    public AssetReference ActiveSceneReference
    {
        get { return _activeSceneReference; }
    }
    public void LoadMainMenu()
    {
        LoadSceneWithoutFade(_mainMenuReference);
    }
    public void LoadSceneWithoutFade(string sceneName) { 
        _sceneName = sceneName;
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadSceneWithoutFade(AssetReference sceneData)
    {
        _activeSceneReference = sceneData;
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                StartCoroutine(SceneLoad((SceneData)sceneData.Asset));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };
    }
    public void LoadSceneWithFade(AssetReference sceneData)
    {
        _activeSceneReference = sceneData;
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                StartCoroutine(SceneLoadWithFade((SceneData)sceneData.Asset));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };
    }
    private IEnumerator FadeAndLoadScene() {
        yield return ScreenFader.Instance.FadeOut();
        SceneManager.LoadScene(_sceneName);
        yield return ScreenFader.Instance.FadeIn();
    }

    private IEnumerator FadeAndLoadAsyncScene() {
        yield return ScreenFader.Instance.FadeOut();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log("grr");
            yield return null;
        }
        yield return ScreenFader.Instance.FadeIn();
    }

    public void LoadScene(string sceneName, bool async = false) {
        _sceneName = sceneName;
        if (async)
            StartCoroutine(FadeAndLoadAsyncScene());
        else
            StartCoroutine(FadeAndLoadScene());
    }
    private IEnumerator SceneLoad(SceneData sceneData)
    {
        Debug.Log("Loading Scene...");
        _activeScene = sceneData;
        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;
        while (!handle.IsDone)
        {
            yield return null;
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            //Rigidbody2D rb = player.GetComponent<CharacterController2D>().Rigidbody;
            //rb.gravityScale = 0.0f;
            //yield return new WaitForSeconds(0.1f);
            //rb.isKinematic = false;
        }
    }

    private IEnumerator SceneLoadWithFade(SceneData sceneData)
    {
        Debug.Log("Loading Scene...");
        _activeScene = sceneData;
        //yield return ScreenFader.Instance.FadeOut();
        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;
        while (!handle.IsDone)
        {
            yield return null;
        }
        //yield return ScreenFader.Instance.FadeIn();
    }

    private void FindAllSceneConnections()
    {
        sceneConnections = GameObject.FindGameObjectsWithTag("SceneConnection");
    }

    private void SpawnPlayer()
    {

    }
}
