using System;

using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerStatData : BaseData
{

    [SerializeField]
    private CheckpointData _checkPointData;

    public CheckpointData CheckPointData
    {
        get { return this._checkPointData; }
        set { this._checkPointData = value; }

    }

    [SerializeField]
    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }


    [SerializeField]
    private int _currentManite;
    public int CurrentManite
    {
        get { return _currentManite; }
        set { _currentManite = value; }
    }

    [SerializeField]
    private bool _hasDash = true;
    public bool HasDash
    {
        get { return _hasDash; }
        set { _hasDash = value; }
    }

    [SerializeField]
    private bool _hasSlash = false;
    public bool HasSlash
    {
        get { return _hasSlash; }
        set { _hasSlash = value; }
    }

    [SerializeField]
    private bool _hasPound = false;
    public bool HasPound
    {
        get { return _hasPound; }
        set { _hasPound = value; }
    }
}
