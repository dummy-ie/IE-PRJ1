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
    private string _targetSceneName;
    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField] private string _nextSpawnPointKey = "default";
    [SerializeField]
    private bool _changeOnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_sceneConnection == SceneConnection.ActiveConnection)
        {
            //if (GameObject.FindGameObjectWithTag("Player") != null)
            //    Destroy(GameObject.FindGameObjectWithTag("Player"));
            if (_spawnPoint != null)
            {
                //PlayerSpawner.Instance.SpawnPlayerAtLocation();
                //FindObjectOfType<CharacterController2D>().LastSpawnPosition = _spawnPoint;
            }
            else
            {
                Debug.LogError("Spawn Point is NULL");
            }
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

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            JSONSave.Instance.SaveAll();
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_changeOnTrigger)
        {
            if (other.CompareTag("Player"))
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
    }
}
