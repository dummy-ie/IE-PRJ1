using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Used with Beam prefab.
public class LaserProjectile : Projectile
{
    [SerializeField]
    [Range(0.1f, 3.0f)]
    private float _shootDelay = 0.5f;

    [SerializeField]
    private GameObject _telegraphVisual;

    private Vector3 _shootDirection;

    protected override void OnExpire()
    {
        Destroy(transform.parent.parent.gameObject);
    }

    // Set the projectile direction. Usually, pass "Vector3.(up, down, left, right)". 
    public void SetDirection(Vector3 direction)
    {
        // Convert direction to nearest cardinal direction. (Just in case you don't pass a cardinal vector.)
        float angle = Mathf.Atan2(direction.y, direction.x);
        int octant = Mathf.RoundToInt(4 * angle / (2 * Mathf.PI) + 4) % 4;

        switch (octant)
        {
            case 0: // East
                direction = Vector3.right;
                break;
            case 1: // South
                direction = Vector3.down;
                break;
            case 2: // West
                direction = Vector3.left;
                break;
            case 4: // North
                direction = Vector3.up;
                break;
        }

        // Debug.Log("Octant: " + octant);

        _shootDirection = direction;

        transform.parent.parent.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg);
        _shootDirection = _shootDirection.normalized * CalculateSpeed();
    }

    protected override void Move()
    {
        if (_lifespanCounter > _shootDelay)
        {
            _telegraphVisual.SetActive(false);
            // transform.parent.Translate(_shootDirection * Time.deltaTime, Space.World);
            transform.parent.localScale += new Vector3(CalculateSpeed(), 0, 0) * Time.deltaTime;
        }
        else if (_shootDirection.x == 0) // North/South Laser stay with source until shot
        {
            transform.parent.parent.position = new(_sourcePlayer.transform.position.x, transform.position.y, transform.position.z);
        }
        else if (_shootDirection.y == 0) // East/West Laser stay with source until shot
        {
            transform.parent.parent.position = new(transform.position.x, _sourcePlayer.transform.position.x, transform.position.z);
        }
        else if (_lifespanCounter <= _shootDelay)
        {
            // flash the telegraph
            _telegraphVisual.SetActive(!_telegraphVisual.activeInHierarchy);
        }
    }
}
