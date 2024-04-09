using JetBrains.Annotations;

public struct HitData
{
    public float damage;
    public float force;
    public HitData(float damage, float force)
    {
        this.damage = damage;
        this.force = force;
    }
}
