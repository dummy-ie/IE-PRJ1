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

    // Start is called before the first frame update
    void Start()
    {
        if (_sceneConnection == SceneConnection.ActiveConnection)
        {
            //PlayerSpawner.Instance.SpawnPlayerAtLocation(_spawnPoint.position);
            FindObjectOfType<CharacterController2D>().LastSpawnPosition = _spawnPoint;
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
        //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
        //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
        if (_targetSceneReference != null)
            SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference);
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            if (_targetSceneReference != null)
                SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference);
        }
    }*/
}
