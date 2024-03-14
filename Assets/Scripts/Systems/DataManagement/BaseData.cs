using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BaseData : ScriptableObject
{
    [SerializeField]
    protected string dataId;
    public string ID
    {
        get { return dataId; }
    }
}
