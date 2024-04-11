using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _faceToRight = false;
    private CharacterController2D _controller;
    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint.gameObject.SetActive(false);
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    public void SetSpawnPoint()
    {
        CharacterSpawnPoint newSpawn = new CharacterSpawnPoint()
        {
            position = _spawnPoint.position,
            faceToRight = _faceToRight
        };
        _controller.LastSpawnPosition = newSpawn;
    }
}
