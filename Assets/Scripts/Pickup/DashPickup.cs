using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnPickup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup?.Invoke();
            //GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().ObtainDash();
            Destroy(gameObject);
        }
    }
}
