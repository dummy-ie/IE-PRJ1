using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DroneState{
    MOVING,
    HOVERING
}

public class LM40DroneBase<TDrone> : EntityStateMachine<TDrone> where TDrone : LM40DroneBase<TDrone>
{

    [System.Serializable]
    protected struct AttackData
    {
        public float moveOffset;
        public Rect attackCollision;
        public int damage;
        public Vector2 knockbackForce;
    }

    [SerializeField]
    protected EnemyData _enemyData;

    [SerializeField]
    protected GameObject _targetPlayer;


    Rigidbody _rb;

    [SerializeField]
    Vector3 _targetLoc;

    [SerializeField]
    protected int floatingValueX = 1;

    [SerializeField]
    protected int floatingValueY = 1;

    [SerializeField]
    protected int floatingSpeedX = 2;

    [SerializeField]
    protected int floatingSpeedY = 2;

    // Start is called before the first frame update
    protected virtual void Start()
    {
       // _targetLoc = transform.position + Vector3.up * 5;
        
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        base.Update();
        /*_targetLoc = test.transform.position;
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
        }*/

    }

    public float GetDirection(GameObject target)
    {
        if (target != null)
        {

            float value = target.transform.position.x - target.transform.position.x;
            if (value < 0)
                return -1;

            else
                return 1;

        }

        return 0;

    }

    protected void Hovering()
    {
        Vector3 vectorForce = (new Vector3(Mathf.Sin(Time.time * floatingSpeedX) * floatingValueX, Mathf.Sin(Time.time * floatingSpeedY) * floatingValueY, 0));

        this._rb.velocity = vectorForce;

        /*if (Vector3.Distance(_targetLoc, transform.position) > 3)
        {
            _state = DroneState.MOVING;
        }*/
    }

    protected void MoveTargetLoc(Vector3 targetLoc)
    {
        this._targetLoc = targetLoc;
    }

    protected float GetDistanceToTarget()
    {
        return Vector3.Distance(_targetLoc, transform.position);
    }

    protected void Moving()
    {
        
        Vector3 vectorForce = _targetLoc - transform.position;

        vectorForce = new Vector3(Mathf.Clamp(vectorForce.x, -2, 2), Mathf.Clamp(vectorForce.y, -2, 2), 0);
        Debug.Log(vectorForce);
        this._rb.velocity = vectorForce * Vector3.Distance(transform.position, _targetLoc);

        /*if(Vector3.Distance(_targetLoc, transform.position) <= 1)
        {
            _state = DroneState.HOVERING;
        }*/
    }
}
