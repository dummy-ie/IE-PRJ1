using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalProjectile : Projectile
{
    private Vector3 _shootDirection;

    public void SetTarget(Transform target)
    {
        _shootDirection = target.position - transform.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg);
        _shootDirection = _shootDirection.normalized * CalculateSpeed();
    }
    protected override void Move()
    {
        transform.Translate(_shootDirection * Time.deltaTime, Space.World);
    }
}
