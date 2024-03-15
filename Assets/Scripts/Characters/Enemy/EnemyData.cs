using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
[System.Serializable]
public class EnemyData : ScriptableObject
{
    //public Sprite spritesheet;

    [SerializeField]
    private string _enemyName;
    public string EnemyName
    {
        get { return _enemyName; }
    }

    [SerializeField]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [SerializeField]
    private int _health = 3;
    public int Health
    {
        get { return _health; }
    }

    [SerializeField]
    private int _baseDamage = 1;
    public int BaseDamage
    {
        get { return _baseDamage; }
    }
}
