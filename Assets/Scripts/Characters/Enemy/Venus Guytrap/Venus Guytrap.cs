using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VenusGuytrapMovement : EnemyBaseScript
{
    private GameObject _target;

    private float _speed = 4;

    private float _flipDirection = 1;

    [SerializeField]
    private EStateEnemy _venusGuytrapState;

    [SerializeField]
    private float _distanceFromTarget = 5f;

    [SerializeField]
    private Vector2 _detectionBox = new Vector2(10f, 1f);

    [SerializeField]
    private GameObject _venusGuytrapProjectile;

    public EStateEnemy VenusGuytrapState
    {
        get { return this._venusGuytrapState; }
        set { this._venusGuytrapState = value; }
    }

    bool _isAttacking = false;
    bool _canAttack = true;
    bool _canMove = true;

    bool _iFramed;

    // Start is called before the first frame update
    void OnEnable()
    {
        this._venusGuytrapState = EStateEnemy.PATROL;
        this.StartCoroutine(FlipInterval());

    }

    void Start()
    {
        this.StartCoroutine(FlipInterval());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();
        if (this._venusGuytrapState == EStateEnemy.ATTACK)
        {
            CheckAttack(Detect());

            if (!_isAttacking)
            {
                Move();
            }
        }
        else if (this._venusGuytrapState == EStateEnemy.PATROL)
        {
            CheckForPlayer(Detect());
            Move();
        }
    }

    void Move()
    {
        if (_canMove && this._venusGuytrapState == EStateEnemy.ATTACK)
        {
            // MaintainDistanceFromTarget();
        }
        else if (_canMove && this._venusGuytrapState == EStateEnemy.PATROL)
        {
            // Vector2 pos = new(transform.position.x + (this.flipDirection * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
            // _rb.velocity = new(Mathf.Lerp(0, _flipDirection * _speed, 0.5f), _rb.velocity.y);
        }
    }

    void MaintainDistanceFromTarget()
    {
        float calculatedDistance = Mathf.Abs(Vector2.Distance(_target.transform.position, transform.position));
        // Debug.Log("Distance from target: " + calculatedDistance);
        if (calculatedDistance < _distanceFromTarget - 0.5f)
        {
            _rb.velocity = new(Mathf.Lerp(0, -GetPlayerDirection() * _speed, 0.5f), _rb.velocity.y);
            // Vector2 pos = new(transform.position.x - (this.GetPlayerDirection() * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
        else if (calculatedDistance > _distanceFromTarget + 0.5f)
        {
            _rb.velocity = new(Mathf.Lerp(0, GetPlayerDirection() * _speed, 0.5f), _rb.velocity.y);
            // Vector2 pos = new(transform.position.x + (this.GetPlayerDirection() * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
    }

    IEnumerator FlipInterval()
    {
        if (this._venusGuytrapState == EStateEnemy.PATROL)
        {

            this._flipDirection = UnityEngine.Random.Range(0, 2);

            if (this._flipDirection == 0)
            {
                this._flipDirection = -1;
            }

        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(FlipInterval());



    }

    IEnumerator AttackShoot()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(.2f);

        if (_isAttacking)
        {
            GameObject projectile = Instantiate(
                _venusGuytrapProjectile,
                new Vector3(transform.position.x + 0.2f * GetPlayerDirection(), transform.position.y + 0.1f, transform.position.z),
                Quaternion.identity);

            // set source and target
            var temp = projectile.GetComponent<DirectionalProjectile>();
            temp.SourcePlayer = gameObject;

            temp.SetTarget(_target.transform);
        }

        yield return new WaitForSeconds(1);

        _isAttacking = false;

        yield return new WaitForSeconds(1);

        _canAttack = true;

    }

    IEnumerator Stagger()
    {
        _iFramed = true;
        _canAttack = _canMove = _isAttacking = false;
        StopCoroutine(AttackShoot());

        yield return new WaitForSeconds(.7f);

        _iFramed = false;
        _canMove = true;

        yield return new WaitForSeconds(.7f);

        _canAttack = true;
    }

    override public void OnHit(Transform source, int damage)
    {
        if (!_iFramed)
        {
            if (this._venusGuytrapState != EStateEnemy.ATTACK)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
                this._venusGuytrapState = EStateEnemy.ATTACK;
            }
            // Vector2 vec = new Vector2(transform.position.x - source.position.x, 0);
            // vec.Normalize();
            // _rb.velocity = Vector2.zero;
            // _rb.AddForce(vec * 5, ForceMode2D.venusGuytrapulse);

            if (damage > 0)
                Damage(damage);

            StartCoroutine(Stagger());

            // Debug.Log("venusGuytrap Hit");
        }
    }

    override protected void Flip()
    {
        float flip;
        if (this._venusGuytrapState == EStateEnemy.ATTACK && _target != null)
        {
            flip = GetPlayerDirection();
        }

        else if (this._venusGuytrapState == EStateEnemy.PATROL)
        {
            flip = this._flipDirection;
        }

        else
        {
            flip = _rb.velocity.x;
        }
        if (_isFacingRight && flip < 0f || !_isFacingRight && flip > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private float GetPlayerDirection()
    {
        float value = _target.transform.position.x - transform.position.x;
        if (value < 0)
            return -1;
        else
            return 1;
    }

    RaycastHit2D[] Detect()
    {
        int flip = 1;
        if (_isFacingRight) flip = -1;
        RaycastHit2D[] hits;


        hits = Physics2D.BoxCastAll(transform.position, _detectionBox, 0, -transform.right * flip, 2.5f);


        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _target = hit.collider.gameObject;
                this._venusGuytrapState = EStateEnemy.ATTACK;
                StopCoroutine(this.FlipInterval());
            }
        }
    }

    void CheckAttack(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                // Debug.Log("venusGuytrap detects player");
                // Debug.Log("venusGuytrap can attack: " + canAttack);
                // Debug.Log("venusGuytrap is grounded: " + IsGrounded());
                if (_canAttack && IsGrounded())

                {
                    StartCoroutine(AttackShoot());
                    _canAttack = false;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        int flip = 1;
        if (_isFacingRight) flip = -1;

        Gizmos.DrawWireCube(transform.position + (2.5f * flip * -transform.right), _detectionBox);
    }
}
