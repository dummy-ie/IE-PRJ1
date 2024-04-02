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


    Rigidbody2D _rb;
    public Rigidbody2D rb
    {
        get {  return _rb; }
    }

    [SerializeField]
    Vector3 _targetLoc;

    [SerializeField]
    protected float floatingValueX = 1;

    [SerializeField]
    protected float floatingValueY = 1;

    [SerializeField]
    protected float floatingSpeedX = 2;

    [SerializeField]
    protected float floatingSpeedY = .1f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
       // _targetLoc = transform.position + Vector3.up * 5;
        
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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

    protected void SetVelocity(Vector2 velocity)
    {
        this._rb.velocity = velocity;
    }

    protected Vector2 Hovering()
    {
        Vector2 vectorForce = (new Vector3(Mathf.Sin(Time.time * floatingSpeedX) * floatingValueX, Mathf.Sin(Time.time * floatingSpeedY) * floatingValueY));

        return vectorForce;

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

    protected Vector2 Moving()
    {
        
        Vector2 vectorForce = (_targetLoc - transform.position) * (Vector3.Distance(transform.position, _targetLoc));

        //vectorForce = new Vector3(Mathf.Clamp(vectorForce.x, -2, 2), Mathf.Clamp(vectorForce.y, -2, 2), 0);
        //Debug.Log(vectorForce);
        return vectorForce;

        /*if(Vector3.Distance(_targetLoc, transform.position) <= 1)
        {
            _state = DroneState.HOVERING;
        }*/
    }
}
