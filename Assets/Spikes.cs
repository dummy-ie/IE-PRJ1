using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerSpawner.Instance.ForceSpawn(SceneManager.GetActiveScene().name, collision.gameObject.GetComponent<CharacterController2D>().LastSpawnPosition.position);
            collision.gameObject.GetComponent<CharacterController2D>().Damage(1);
        }
    }
}
