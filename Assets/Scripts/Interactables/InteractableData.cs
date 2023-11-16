using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
[Serializable]
public class InteractableData : ScriptableObject
{

    public string objectName;
    public Sprite _sprite;
    public bool _wasInteracted = false;


}

