using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalProjectile : Projectile
{
    protected override void Move()
    {
        transform.Translate(new Vector3(CalculateSpeed() * transform.localScale.x * Time.deltaTime, 0, 0), Space.World);
    }
}
