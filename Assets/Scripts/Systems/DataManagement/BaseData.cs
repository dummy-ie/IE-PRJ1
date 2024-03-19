using System;
using UnityEngine;


[Serializable]
public class BaseData : MonoBehaviour
{
    [SerializeField]
    protected string dataId;
    public string ID
    {
        get { return dataId; }
    }
}
