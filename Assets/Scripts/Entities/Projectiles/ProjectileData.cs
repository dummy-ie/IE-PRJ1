using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
[Serializable]
public class ProjectileData : ScriptableObject
{
    [Range(0.1f, 60f)]
    public float LifespanTime = 1f;

    [Range(0.1f, 100f)]
    public float Speed = 1f;

    public int Damage = 1;

    public bool DestroyOnImpactWithTarget = true;

    public bool DestroyOnCollisionWithGround = true;
}
