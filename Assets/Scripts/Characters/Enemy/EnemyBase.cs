using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBase : MonoBehaviour, IHittable
{
    public enum State
    {
        Idle, //not moving
        Patrol, //moving around
        Engaging, //positioning for attack against player
        Attacking //trigger attack
    }

    protected Rigidbody2D _rb;


    [SerializeField]
    protected EnemyData _enemyData;

    [SerializeField]
    protected int _currentHealth;
    protected int _speed = 1;

    [SerializeField]
    protected State _currentState = State.Patrol;
    public State CurrentState 
    { 
        get { return _currentState; } 
        set { _currentState = value; }
    }

    protected Collider2D _visionCollider;

    [SerializeField]
    GameObject _manitePrefab;

    private Material _mat;

    protected bool _isFacingRight = true;
    protected Vector3 _patrolDirection = Vector3.right;
    public Vector3 PatrolDirection
    {
        get { return _patrolDirection; }
        set { _patrolDirection = value; }
    }

    protected float _patrolTick;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);
    [SerializeField] LayerMask _groundLayer;

    virtual public void OnHit(Transform source, int damage)
    {
        Stagger(source);
        //StartCoroutine(Blink());
        Debug.Log("Hit");
    }

    protected void Stagger(Transform source)
    {
        Vector2 vec = new Vector2(transform.position.x - source.position.x, transform.position.y - source.position.y);
        vec.Normalize();
        _rb.AddForce(vec * 5, ForceMode2D.Impulse);
    }

    protected virtual void Patrol() 
    {

        Vector3 pos = new(transform.position.x + (this._patrolDirection.x * _speed), transform.position.y + (this._patrolDirection.y * _speed), 0);
        transform.position = Vector3.MoveTowards(_rb.transform.position, pos, Time.deltaTime * _speed);


    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _groundLayer);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _enemyData.Health;
        _mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._currentHealth <= 0)
        {
            Debug.Log("Enemy Killed");
            Instantiate(_manitePrefab, transform.position, transform.rotation);
            this.gameObject.SetActive(false); //OR Destroy(this.gameObject);
        }

        if (CurrentState == State.Patrol)
        {
            Patrol();
        }

        Flip();
    }

    virtual protected void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = PatrolDirection.x;
        transform.localScale = localScale;
    }
}
