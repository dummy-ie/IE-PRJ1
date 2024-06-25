using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectBehaviour : MonoBehaviour
{
    //[SerializeField]
    //EnemyBase _enemyScript;
    public bool WallDetected = false;
    void Awake()
    {
        //_enemyScript = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            WallDetected = true;
        }
        /*if (collision.gameObject.layer == 6 && _enemyScript.CurrentState == EnemyBase.State.Patrol)
        {
            _enemyScript.PatrolDirection *= -1;
        }*/
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            WallDetected = false;
        }
    }
}
