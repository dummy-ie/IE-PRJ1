using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
[Serializable]
public class InteractableData : BaseData
{
    /*[SerializeField]
    private Sprite _sprite;
    public Sprite Sprite
    {

        get { return this._sprite; }

        set { this._sprite = value; }
    }*/

    [SerializeField]
    private bool _enabled;
    public bool Enabled
    {

        get { return this._enabled; }

        set { _enabled = value; }
    }

    InteractableData(string _objectName, bool _enabled)
    {
        this._objectName = _objectName;
        //this._sprite = _sprite;
        this._enabled = _enabled;
    }

    /*
    private void AfterDeserializeField(){
        Debug.Log("Values succesfully reset.");
        this._objectName  = this.baseName;
        this._sprite = this._baseSprite;
        this._wasInteracted = this._baseState;
    }
    */

}

