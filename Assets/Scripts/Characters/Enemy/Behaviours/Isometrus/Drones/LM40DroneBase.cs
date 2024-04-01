using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DroneState{
    MOVING,
    HOVERING
}

public class LM40DroneBase : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField]
    Vector3 _targetLoc;

    [SerializeField]
    protected DroneState _state = DroneState.HOVERING;

    [SerializeField]
    protected int floatingValueX = 2;

    [SerializeField]
    protected int floatingValueY = 6;

    [SerializeField]
    GameObject test;

    // Start is called before the first frame update
    void Start()
    {
        _targetLoc = transform.position + Vector3.up * 5;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _targetLoc = test.transform.position;
        switch (_state)
        {
            case DroneState.MOVING:
                Moving();
                break;

            case DroneState.HOVERING:
                Hovering();
                break;

            default:
                break;
        }

    }

    private void Hovering()
    {
        Vector3 vectorForce = (new Vector3(Mathf.Sin(Time.time * floatingValueX), Mathf.Sin(Time.time * floatingValueY), 0));

        this._rb.velocity = vectorForce;

        if (Vector3.Distance(_targetLoc, transform.position) > 3)
        {
            _state = DroneState.MOVING;
        }
    }

    protected void MoveTargetLoc(Vector3 targetLoc)
    {
        this._targetLoc = targetLoc;
    }

    protected void Moving()
    {
        
        Vector3 vectorForce = _targetLoc - transform.position;

        vectorForce = new Vector3(Mathf.Clamp(vectorForce.x, -2, 2), Mathf.Clamp(vectorForce.y, -2, 2), 0);
        Debug.Log(vectorForce);
        this._rb.velocity = vectorForce * Vector3.Distance(transform.position, _targetLoc);

        if(Vector3.Distance(_targetLoc, transform.position) <= 1)
        {
            _state = DroneState.HOVERING;
        }
    }
}
