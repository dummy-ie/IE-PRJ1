using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Vector3 velocity;
    private GameObject entity;

    private SlimeMovement slimeBehavior;
    
    private Rigidbody rb;
    private float ticks;

   
    private void OnEnable(){

        entity = this.transform.parent.gameObject;


    }

    private void OnTriggerEnter2D(Collider2D col)
    {   
        
        if(col.gameObject.CompareTag("Player")){
            Debug.Log("Triggered");
        }
    }

    

}

