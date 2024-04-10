using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private SceneConnection _sceneConnection;
    [SerializeField]
    private AssetReference _targetSceneReference;
    [SerializeField]
    private string _nextSpawnPointKey = "default";

    // Start is called before the first frame update
    void Start()
    {
        if (_sceneConnection == SceneConnection.ActiveConnection)
        {

        }

        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsActiveConnection()
    {
        return _sceneConnection == SceneConnection.ActiveConnection;
    }

    public void LoadNextScene()
    {
        SceneConnection.ActiveConnection = _sceneConnection;
        SceneLoader.TransitionData transitionData = new SceneLoader.TransitionData
        {
            spawnPoint = _nextSpawnPointKey
        };
        if (_targetSceneReference != null)
            SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference, transitionData);
    }
}
