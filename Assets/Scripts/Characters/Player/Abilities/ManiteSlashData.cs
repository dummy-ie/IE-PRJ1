using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManiteSlashData", menuName = "Scriptable Objects/Characters/Player/Abilities/ManiteSlash")]
public class ManiteSlashData : ScriptableObject
{
    [Header("Manite Slash Properties")]

    [SerializeField]
    private int _maniteSlashCost = 20;
    public int ManiteSlashCost
    {
        get { return _maniteSlashCost; }
    }

    [SerializeField]
    private float _maniteSlashDamage = 5f;
    public float ManiteSlashDamage
    {
        get { return _maniteSlashDamage; }
    }

    [SerializeField]
    private float _maniteSlashCooldown = 1.5f; 
    public float ManiteSlashCooldown
    {
        get { return _maniteSlashCooldown; }
    }
}
