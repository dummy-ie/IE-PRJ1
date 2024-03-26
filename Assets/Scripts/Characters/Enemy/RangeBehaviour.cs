using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBase _enemy;

    public bool InRange = false;
    void Awake()
    {
        _enemy = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //_enemy.ChangeState() = EnemyBase.State.Attacking;
            InRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            //_enemy.CurrentState = EnemyBase.State.Engaging;
            InRange = false;
        }
    }
}
