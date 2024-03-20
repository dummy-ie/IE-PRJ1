using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DoorController : MonoBehaviour, ISaveable
{
    [SerializeField]
    InteractableData _interactableData;
    private SpriteRenderer _renderer;
    [SerializeField]
    private Sprite _openSprite;
    [SerializeField]
    private Sprite _closedSprite;
    [SerializeField]
    private bool _enabled;
    private Collider2D _collider;
    public void OnInteract(){
        if(this._enabled == true){
            this._collider.enabled = true;
            this._enabled = false;
        }

        else {
            this._collider.enabled = false;
            this._enabled = true;
        }

        CheckDoorRender();
        //"Save" the data if the object was interacted or not      
        this._interactableData.Enabled = this._enabled;

        
    }

    private void CheckDoorRender(){

        if(this._enabled == true){
            this._renderer.sprite = this._openSprite;
            this._collider.enabled = false;
        }

        else if(this._enabled == false){
            this._renderer.sprite = this._closedSprite;
        }
    }


    private void Awake(){
        this._collider = this.gameObject.GetComponent<Collider2D>();
        this._renderer = this.gameObject.GetComponent<SpriteRenderer>();

        //gets the info if the door was opened or closed beforehand, to "save" the memory of the door being opened
        StartCoroutine(LoadBuffer());
    }

    private void OnEnable()
    {
        CheckDoorRender();

    }

    private IEnumerator LoadBuffer()
    {
        yield return new WaitForSeconds(.1f);
        LoadData();

        this._enabled = this._interactableData.Enabled;
        CheckDoorRender();
    }

    public void LoadData()
    {
        JSONSave.Instance.LoadData<InteractableData>(ref this._interactableData);
    }

    public void SaveData()
    {
        JSONSave.Instance.SaveData(this._interactableData);
    }
}
