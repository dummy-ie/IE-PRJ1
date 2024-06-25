using System;
using UnityEngine;

[Serializable]
public class RangeInt
{
    private int min, max;

    public int Min { get { return min; } }
    public int Max { get { return max; } }
    public int RandomRange() => UnityEngine.Random.Range(min, max + 1);
    public RangeInt(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}