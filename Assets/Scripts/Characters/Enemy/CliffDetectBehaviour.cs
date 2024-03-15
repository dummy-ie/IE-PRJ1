using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffDetectBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBase _enemyScript;

    [SerializeField]
    private List<Collider2D> detectedColliders;

    void Awake()
    {
        _enemyScript = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 6 && _enemyScript.CurrentState == EnemyBase.State.Patrol)
        {
            detectedColliders.Add(collision);
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 6 && _enemyScript.CurrentState == EnemyBase.State.Patrol)
        {
            detectedColliders.Remove(collision);
            if (_enemyScript.IsGrounded() && detectedColliders.Count == 0)
            {
                _enemyScript.PatrolDirection *= -1;
            }
        }
    }
}
