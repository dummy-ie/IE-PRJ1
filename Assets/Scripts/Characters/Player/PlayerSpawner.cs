using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerSpawner : Singleton<PlayerSpawner>
{   
    [SerializeField]
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SpawnPlayerFromSpawnPoint();
    }
    private void InitializePlayer(Vector3 position)
    {
        //GameObject player = Instantiate(_player);
        _player.transform.position = position;
        _player.GetComponent<CharacterController2D>().SetVirtualCameraBoundingBox(GameObject.FindGameObjectWithTag("SceneBounds").GetComponent<CompositeCollider2D>());
    }
    public void SpawnPlayerFromSpawnPoint()
    {
        foreach (GameObject sceneConnectionObject in GameObject.FindGameObjectsWithTag("SceneConnection"))
        {
            SceneChanger sceneChanger = sceneConnectionObject.GetComponent<SceneChanger>();
            if (sceneChanger.IsActiveConnection())
                return;
        }
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPoint.Length == 0)
            return;
        InitializePlayer(spawnPoint[0].transform.position);
    }

    public void SpawnPlayerAtLocation(Vector3 position)
    {
        InitializePlayer(position);
    }
    public void Respawn(string checkpointName, Vector3 position){
        SceneLoader.Instance.LoadScene(checkpointName, true);
        InitializePlayer(position);
    }
    
    public void ForceSpawn(string forcedAreaName, Vector3 forcedSpawnPosition){
        SceneLoader.Instance.LoadScene(forcedAreaName, true);
        InitializePlayer(forcedSpawnPosition);
    }

    public void Respawn(AssetReference sceneReference, Vector3 position)
    {
        SceneLoader.Instance.LoadSceneWithFade(sceneReference);
        InitializePlayer(position);
    }

    public void ForceSpawn(AssetReference sceneReference, Vector3 forcedSpawnPosition)
    {
        SceneLoader.Instance.LoadSceneWithFade(sceneReference);
        InitializePlayer(forcedSpawnPosition);
    }


}
