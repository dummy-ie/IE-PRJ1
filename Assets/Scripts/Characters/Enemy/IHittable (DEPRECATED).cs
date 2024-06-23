using UnityEngine;

public interface IHittable
{
    public void OnHit(Transform source, int damage);
}