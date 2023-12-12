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

    private void OnCollisionEnter(Collision other){

       
        if(other.gameObject.CompareTag("XD1")){
            
            if(other.gameObject != null){

                Debug.Log("Object destroyed");
                _maniteAdd.PickUp = true;
                Destroy(gameObject);

            }
            

        }

    }
}
