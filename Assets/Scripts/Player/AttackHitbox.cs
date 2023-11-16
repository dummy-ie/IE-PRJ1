using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private PlayerAttack attackScript;
    // Start is called before the first frame update
    void Start()
    {
        attackScript = transform.parent.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //attackScript.HitDetected(collision.gameObject.GetComponent<EnemyBaseScript>());
       
    }
}
