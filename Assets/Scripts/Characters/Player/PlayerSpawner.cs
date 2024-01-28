using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class PlayerSpawner : Singleton<PlayerSpawner>
{   
    [SerializeField]
    private GameObject _player;



    public void Respawn(string checkpointName, Vector3 position){
        Debug.Log("MissCandy");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadScene(checkpointName, true);
        _player.transform.position = position;

    }
    
    public void ForceSpawn(string forcedAreaName, Vector3 forcedSpawnPosition){
        Debug.Log("MissCandyButForced");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadScene(forcedAreaName, true);
        _player.transform.position = forcedSpawnPosition;
    }

    public void Respawn(AssetReference sceneReference, Vector3 position)
    {
        Debug.Log("MissCandy");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadSceneWithFade(sceneReference);
        _player.transform.position = position;

    }

    public void ForceSpawn(AssetReference sceneReference, Vector3 forcedSpawnPosition)
    {
        Debug.Log("MissCandyButForced");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadSceneWithFade(sceneReference);
        _player.transform.position = forcedSpawnPosition;
    }


}
