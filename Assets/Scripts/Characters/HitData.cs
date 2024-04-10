using UnityEngine;
public struct HitData
{
    public float damage;
    public Vector2 force;
    public HitData(float damage, Vector2 force)
    {
        this.damage = damage;
        this.force = force;
    }
}
