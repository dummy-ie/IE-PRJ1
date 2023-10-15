using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collider){

       
        if(collider.gameObject.CompareTag("XD1")){
            
            if(collider.gameObject != null){

                Debug.Log("Object destroyed");
                Destroy(gameObject);

            }
            

        }

    }
}
