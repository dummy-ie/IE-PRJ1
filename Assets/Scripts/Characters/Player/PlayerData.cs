using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerData : BaseData
{
    // have default values for all fields to prevent null errors
    [Header("Properties")]
    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField]
    private int _maxManite = 100;
    public int MaxManite {
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
        set { _groundLayer = value; }
    }

    [Header("Movement")]
    [Range(0, 100)]
    [SerializeField]
    private float _speed = 6f;
    public float Speed { 
        get { return _speed; } 
        set { _speed = value; }
    }

    [Header("Jumping")]
    [SerializeField]
    private bool _allowDoubleJump = false;
    public bool AllowDoubleJump { 
        get { return _allowDoubleJump; } 
        set { _allowDoubleJump = value; }
    }

    [Range(0, 100)]
    [SerializeField]
    private float _jumpForce = 17f;
    public float JumpForce { 
        get { return _jumpForce; } 
        set { _jumpForce = value; }
    }

    [Range(0, 10)]
    [SerializeField]
    private float _fallMultiplier = 4f, _lowJumpMultiplier = 0.3f;
    public float FallMultiplier { 
        get { return _fallMultiplier; } 
        set { _fallMultiplier = value; }
    }
    public float LowJumpMultiplier {
        get { return _lowJumpMultiplier; }
        set { _lowJumpMultiplier = value; }
    }

    [Range(0, 5)]
    [SerializeField]
    private float _coyoteTime = 0.1f, _jumpBufferTime = 0.2f;
    public float CoyoteTime { 
        get { return _coyoteTime; } 
        set { _coyoteTime = value;}
    }
    public float JumpBufferTime {
        get { return _jumpBufferTime; }
        set { _jumpBufferTime = value;}
    }

    [Header("Dashing")]
    [SerializeField]
    private float _dashOriginalSpeed = 20f;
    public float DashOriginalSpeed {
        get { return _dashOriginalSpeed; }
        set { _dashOriginalSpeed = value; }
    }
    [SerializeField]
    private float _dashCooldown = .5f;
    public float DashCooldown {
        get { return _dashCooldown; }
        set { _dashCooldown = value; }
    }
    [SerializeField]
    private float _dashDistance = 4f;
    public float DashDistance {
        get { return _dashDistance; }
        set { _dashDistance = value; }
    }

    [Header("Attacking")]

    [SerializeField]
    private bool _canAttack = true;
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }

    [SerializeField]
    private float _attackCooldown = .3f;
    public float AttackCooldown
    {
        get { return _attackCooldown; }
        set { _attackCooldown = value; }
    }

    /*[SerializeField]
    private bool _isAttacking = false;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }*/


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
