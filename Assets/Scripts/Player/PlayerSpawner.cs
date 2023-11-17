using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : Singleton<PlayerSpawner>
{   
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private string _checkPointName;

    public string CheckPointName {
        get { return this._checkPointName; }
        set { this._checkPointName = value; }
    }

    private Vector3 _respawnPosition;

    public Vector3 RespawnPosition{
        get { return this._respawnPosition; }
        set { this._respawnPosition = value; }
    }


    public void Respawn(){
        Debug.Log("MissCandy");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadScene(_checkPointName, true);
        _player.transform.position = _respawnPosition;

    }
    
    public void ForceSpawn(string forcedAreaName, Vector3 forcedSpawnPosition){
        Debug.Log("MissCandy");
        _player = GameObject.FindGameObjectWithTag("Player");
        SceneLoader.Instance.LoadScene(forcedAreaName, true);
        _player.transform.position = forcedSpawnPosition;
    }


}
