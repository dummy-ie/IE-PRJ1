using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBase _enemy;

    void Awake()
    {
        _enemy = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _enemy.CurrentState = EnemyBase.State.Attacking;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            _enemy.CurrentState = EnemyBase.State.Engaging;
        }
    }
}
