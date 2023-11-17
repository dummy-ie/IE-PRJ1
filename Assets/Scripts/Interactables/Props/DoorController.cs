using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    InteractableData _interactableData;
    private SpriteRenderer _renderer;
    private Sprite _tempSprite;
    private bool _wasUsed;
    private Collider2D _collider;
    public void OnInteract(){
        if(this._wasUsed == true){
            this._collider.enabled = true;
            this._wasUsed = false;
        }

        else {
            this._collider.enabled = false;
            this._wasUsed = true;
        }

        //"Save" the data if the object was interacted or not      
        this._interactableData._wasInteracted = this._wasUsed;
    }

    private void CheckDoorRender(){
        if(this._wasUsed == true){
            
            this._renderer.sprite = this._interactableData._sprite;
        }
        else{
            this._renderer.sprite = this._tempSprite;
        }
    }

    
    private void Awake(){

        
        this._renderer = this.gameObject.GetComponent<SpriteRenderer>();
        
         //gets the info if the door was opened or closed beforehand, to "save" the memory of the door being opened
        this._wasUsed = this._interactableData._wasInteracted;
        this._collider = this.gameObject.GetComponent<Collider2D>();
        this._tempSprite = this._renderer.sprite;
    }

    private void Update(){
        CheckDoorRender();
    }
}
