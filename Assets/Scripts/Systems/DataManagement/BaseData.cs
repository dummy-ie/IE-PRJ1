using System;
using UnityEngine;


[Serializable]
public class BaseData 
{
    [SerializeField]
    protected string dataId;
    public string ID
    {
        get { return dataId; }
    }
}
