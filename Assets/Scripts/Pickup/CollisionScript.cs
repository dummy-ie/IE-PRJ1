using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private ManiteAdd _maniteAdd;
    
    private void Start() {
        _maniteAdd = GameObject.Find("Pickup").GetComponent<ManiteAdd>();
    }

    private void OnEnable() {
        _maniteAdd = GameObject.Find("Pickup").GetComponent<ManiteAdd>();
    }

    private void OnTriggerEnter2D(Collider2D collider){

       
        if(collider.gameObject.CompareTag("XD1")){
            
            if(collider.gameObject != null){

                Debug.Log("Object destroyed");
                _maniteAdd.PickUp = true;
                Destroy(gameObject);

            }
            

        }

    }
}
