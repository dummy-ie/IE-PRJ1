using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Scriptable Objects/Characters/Player/Actions/Attack")]
[Serializable]
public class PlayerAttackData : ScriptableObject
{
    [Header("Player Attack Properties")]

    [SerializeField]
    private int _normalAttackDamage = 2;
    public int NormalAttackDamage
    {
        get { return _normalAttackDamage; }
    }

    [SerializeField]
    private float _normalAttackCooldown = .3f;
    public float NormalAttackCooldown
    {
        get { return _normalAttackCooldown; }
    }
}
