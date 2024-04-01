using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToActivate;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _objectToActivate.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
