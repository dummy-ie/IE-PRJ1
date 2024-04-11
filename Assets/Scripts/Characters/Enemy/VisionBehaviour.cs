using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class VisionBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject detectedObject;
    //EnemyBase _enemy;

    public bool PlayerSeen = false;

    void Awake()
    {
        //_enemy = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //_enemy.CurrentState = EnemyBase.State.Engaging;
            if (!collision.gameObject.GetComponent<CharacterController2D>().IsInvisible)
            {
                PlayerSeen = true;
                detectedObject = collision.gameObject;
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //_enemy.CurrentState = EnemyBase.State.Engaging;
            if (collision.gameObject.GetComponent<CharacterController2D>().IsInvisible)
            {
                PlayerSeen = false;
            }
            else
            {
                PlayerSeen = true;
            }

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            //_enemy.CurrentState = EnemyBase.State.Idle;
            PlayerSeen = false;
        }
    }
}
