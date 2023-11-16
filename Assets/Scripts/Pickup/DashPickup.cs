using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().ObtainDash();
    }
}
