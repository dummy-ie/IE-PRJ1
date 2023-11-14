using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Vector3 _velocity;
    private GameObject _entity;

    private SlimeMovement _slimeBehavior;
    
    private Rigidbody _rb;
    private float _ticks;

   
    private void OnEnable(){

        _entity = this.transform.parent.gameObject;


    }

    private void OnTriggerEnter2D(Collider2D col)
    {   
        
        if(col.gameObject.CompareTag("Player")){
            Debug.Log("Triggered");
        }
    }

    

}

