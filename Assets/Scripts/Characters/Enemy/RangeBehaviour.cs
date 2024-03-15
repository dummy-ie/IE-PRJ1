using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBase _enemyScript;

    void Awake()
    {
        _enemyScript = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _enemyScript.CurrentState = EnemyBase.State.Attacking;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            _enemyScript.CurrentState = EnemyBase.State.Engaging;
        }
    }
}
