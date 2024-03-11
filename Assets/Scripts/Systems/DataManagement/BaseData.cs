using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseData
{
    [SerializeField]
    protected string _objectName;
    public string ObjectName
    {

        get { return this._objectName; }

        set { this._objectName = value; }
    }
}
