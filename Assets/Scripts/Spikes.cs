using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceRespawnTrap : MonoBehaviour
{
    [SerializeField]
    int _damage = 1;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerSpawner.Instance.ForceSpawn(SceneManager.GetActiveScene().name, collision.gameObject.GetComponent<CharacterController2D>().LastSpawnPosition.position);
            collision.gameObject.GetComponent<CharacterController2D>().Damage(_damage);
        }
    }
}
