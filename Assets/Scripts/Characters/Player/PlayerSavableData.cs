using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavableData : BaseData
{
    public bool newData = true;
    public float LastSpawnX;
    public float LastSpawnY;
    public float CheckpointPosX;
    public float CheckpointPosY;
    public int Health;
    public int Manite;
    public bool HasThrust;
    public bool HasDash;
    public bool HasSlash;
    public bool HasPound;
    public bool HasInvisibility;
}
