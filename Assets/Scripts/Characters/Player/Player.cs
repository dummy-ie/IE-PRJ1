using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {
    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    private int _health = 3;
    public int Health {
        get { return _health; }
        set { _health = value; }
    }
    [SerializeField]
    private float _maxManite = 100;
    public float MaxManite {
        get { return _maxManite; }
        set { _maxManite = value; }
    }
    [SerializeField]
    private float _manite = 100;
    public float Manite {
        get { return _manite; }
        set { _manite = value; }
    }
    [SerializeField]
    private Vector3 _position;
    public Vector3 Position {
        get { return _position; }
        set { _position = value; }
    }
    [SerializeField]
    private string _currentSceneName;
    public string CurrentSceneName {
        get { return _currentSceneName; }
        set { _currentSceneName = value; }
    }
    [SerializeField]
    private bool _hasDash = false;
    public bool HasDash {
        get { return _hasDash; }
        set { _hasDash = value; }
    }
    [SerializeField]
    private bool _hasSlash = false;
    public bool HasSlash {
        get { return _hasSlash; }
        set { _hasSlash = value; }
    }
}
