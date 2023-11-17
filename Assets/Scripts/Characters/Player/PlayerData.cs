using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Characters/Player Data")]
public class PlayerData : ScriptableObject {
    // have default values for all fields to prevent null errors
    [Header("Properties")]
    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    private float _maxManite = 100;
    public float MaxManite {
        get { return _maxManite; }
        set { _maxManite = value; }
    }
    /*[SerializeField]
    private int _health = 3;
    public int Health {
        get { return _health; }
        set { _health = value; }
    }
    [SerializeField]
    private float _manite = 100;
    public float Manite {
        get { return _manite; }
        set { _manite = value; }
    }*/

    [Header("Ground Check")]
    [SerializeField] LayerMask _groundLayer;
    public LayerMask GroundLayer { 
        get { return _groundLayer; } 
    }

    [Header("Movement")]
    [Range(0, 100)]
    [SerializeField]
    private float _speed = 6f;
    public float Speed { 
        get { return _speed; } 
    }

    [Header("Jumping")]
    [SerializeField]
    private bool _allowDoubleJump = false;
    public bool AllowDoubleJump { 
        get { return _allowDoubleJump; } 
    }

    [Range(0, 100)]
    [SerializeField]
    private float _jumpForce = 14f;
    public float JumpForce { 
        get { return _jumpForce; } 
    }

    [Range(0, 10)]
    [SerializeField]
    private float _fallMultiplier = 4f, _lowJumpMultiplier = 0.8f;
    public float FallMultiplier { 
        get { return _fallMultiplier; } 
    }
    public float LowJumpMultiplier {
        get { return _lowJumpMultiplier; }
    }

    [Range(0, 5)]
    [SerializeField]
    private float _coyoteTime = 0.2f, _jumpBufferTime = 0.2f;
    public float CoyoteTime { 
        get { return _coyoteTime; } 
    }
    public float JumpBufferTime {
        get { return _jumpBufferTime; }
    }

    [Header("Dashing")]
    [SerializeField]
    private float _dashOriginalSpeed = 20f;
    public float DashOriginalSpeed {
        get { return _dashOriginalSpeed; }
    }
    [SerializeField]
    private float _dashCooldown = .5f;
    public float DashCooldown {
        get { return _dashCooldown; }
    }
    [SerializeField]
    private float _dashDistance = 4f;
    public float DashDistance {
        get { return _dashDistance; }
    }

    /*[SerializeField]
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
    }*/
}
