using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[Serializable]
[JsonObject]
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
    private int _maxManite = 100;
    public int MaxManite {
        get { return _maxManite; }
        set { _maxManite = value; }
    }
    [Serializable]
    public class Attack
    {
        [SerializeField]
        private int _damage = 1;
        public int Damage
        {
            get { return _damage; }
        }

        [SerializeField]
        private float _cooldown = .3f;
        public float Cooldown
        {
            get { return _cooldown; }
        }

        [SerializeField] private Vector2 _hitboxSize;

        public Vector2 HitboxSize
        {
            get { return _hitboxSize; }
        }

        [SerializeField] private float _hitboxCastDistance;

        public float HitboxCastDistance
        {
            get { return _hitboxCastDistance; }
        }
    }


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

    [Header("Attacking")]
    public Attack FirstAttack;
    [SerializeField]
    private bool _canAttack = true;
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    [SerializeField]
    private float _actionCooldown;
    public float ActionCooldown
    {
        get { return _actionCooldown; }
        set { _actionCooldown = value; }
    }
}
