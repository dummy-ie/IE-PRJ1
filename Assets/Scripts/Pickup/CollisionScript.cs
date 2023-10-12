using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
   

    private void OnCollisionEnter2D(Collision2D collider){

       
        if(collider.gameObject.CompareTag("Player")){
            
            Debug.Log("Object destroyed");
            Destroy(gameObject);

        }

    }
}
