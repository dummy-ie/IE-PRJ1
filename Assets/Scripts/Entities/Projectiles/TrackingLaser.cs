using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingLaser : MonoBehaviour
{
    private bool _tracking = false;
    [SerializeField] private Transform _target;
    private Vector2 _direction;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private Vector2 _raycastDirection;

    [SerializeField] private LineRenderer _line;

    public void StartTracking()
    {
        _tracking = true;
    }

    public void StopTracking()
    {
        _tracking = false;
    }

    // Update is called once per frame
    void Update()
    {

        _direction = _target.position - transform.position;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

        _raycastDirection = transform.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _raycastDirection);


        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, _raycastDirection * 10);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
                _line.SetPosition(1, hit.point);

        }
    }
}
