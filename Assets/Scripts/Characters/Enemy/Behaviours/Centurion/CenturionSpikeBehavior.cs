using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenturionSpikeBehavior : MonoBehaviour
{
    [SerializeField]
    int _damage = 1;

    bool _isActivated = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && this._isActivated == true) {
            collision.gameObject.GetComponent<CharacterController2D>().Damage(_damage);
        }
    }
}
