using System;
using UnityEngine;

[Serializable]
public class RangeFloat
{
    [SerializeField]
    private float min, max;

    public float Min { get { return min; } }
    public float Max { get { return max; } }
    public float RandomRange() => UnityEngine.Random.Range(min, max);
    public RangeFloat(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}