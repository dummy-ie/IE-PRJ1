using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
[System.Serializable]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public string description;
    public Sprite spritesheet;
    public int health = 3;
    public int damage = 1;
}
