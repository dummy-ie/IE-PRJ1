using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private ManiteAdd maniteAdd;
    
    private void Start() {
        maniteAdd = GameObject.Find("Pickup").GetComponent<ManiteAdd>();
    }

    private void OnEnable() {
        maniteAdd = GameObject.Find("Pickup").GetComponent<ManiteAdd>();
    }

    private void OnTriggerEnter2D(Collider2D collider){

       
        if(collider.gameObject.CompareTag("XD1")){
            
            if(collider.gameObject != null){

                Debug.Log("Object destroyed");
                maniteAdd.PickUp = true;
                Destroy(gameObject);

            }
            

        }

    }
}
