using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[Serializable]
[JsonObject]
public class PlayerData : ScriptableObject {
    // have default values for all fields to prevent null errors
    [Serializable]
    public class Attack
    {
        public int Damage = 1;
        public float Cooldown = 0.3f;
        public float Duration = 0.3f;
        public float HorizontalMoveOffset;
        public Rect TriggerRect;
    }

    [Header("Properties")]
    public int MaxHealth = 100;
    public int MaxManite = 100;
    
    [Header("Ground Check")]
    public LayerMask GroundLayer;

    [Header("Movement")]
    [Range(0, 100)]
    public float Speed = 6f;

    [Header("Jumping")]
    public bool AllowDoubleJump = false;

    [Range(0, 100)]
    public float JumpForce = 14f;

    [Range(0, 10)]
    [SerializeField]
    public float FallMultiplier = 4f, LowJumpMultiplier = 0.8f;

    [Range(0, 5)]
    [SerializeField]
    public float CoyoteTime = 0.2f, JumpBufferTime = 0.2f;

    [Header("Dashing")]
    public float DashOriginalSpeed = 20f;
    public  float DashCooldown = .5f;
    public float DashDistance = 4f;

    [Header("Attacks")]
    public Attack FirstAttack;

    [Header("Action Cooldown")]
    public float ActionCooldown;

    [Header("Invincibility")]
    public float invincibilityFlickerChange;
    public float defaultInvincibilityTime;
}
