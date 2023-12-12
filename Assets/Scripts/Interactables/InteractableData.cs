using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
[Serializable]
public class InteractableData : ScriptableObject
{   
    [SerializeField]
    private string baseName;

    [SerializeField]
    private Sprite _baseSprite;

    [SerializeField]
    private bool _baseState;

    private string _objectName;
    public string ObjectName{

        get{ return this._objectName; }

        set { this._objectName = value; }
    }
    
    private Sprite _sprite;

    public Sprite Sprite{

        get{ return this._sprite; }

        set { this._sprite = value; }
    }
    
    private bool _wasInteracted;

    public bool WasInteracted{

        get{ return this._wasInteracted; }

        set { _wasInteracted = value; }
    }
    

    private void OnEnable(){

        this._objectName  = this.baseName;
        this._sprite = this._baseSprite;
        this._wasInteracted = this._baseState;
        
    }

    private void AfterDeserializeField(){
        Debug.Log("Values succesfully reset.");
        this._objectName  = this.baseName;
        this._sprite = this._baseSprite;
        this._wasInteracted = this._baseState;
    }


}

