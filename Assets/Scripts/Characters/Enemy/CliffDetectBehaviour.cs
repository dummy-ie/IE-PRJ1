using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffDetectBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Collider2D> detectedColliders;

    public bool CliffDetected = false;

    void Awake()
    {
        //_enemyScript = GetComponentInParent<EnemyBase>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 6)
        {
            detectedColliders.Add(collision);
            if (detectedColliders.Count > 0)
                CliffDetected = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 6)
        {
            detectedColliders.Remove(collision);
            if (detectedColliders.Count == 0)
                CliffDetected = true;
        }
    }
}
