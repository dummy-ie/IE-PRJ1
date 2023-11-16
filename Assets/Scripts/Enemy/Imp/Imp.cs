using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ImpMovement : EnemyBaseScript
{
    private GameObject _target;

    private float _speed = 4;

    private float _flipDirection = 1;

    [SerializeField]
    private EStateEnemy _impState;

    [SerializeField]
    private float _distanceFromTarget = 5f;

    [SerializeField]
    private GameObject _impProjectile;

    public EStateEnemy ImpState
    {
        get { return this._impState; }
        set { this._impState = value; }
    }

    bool _isAttacking = false;
    bool _canAttack = true;
    bool _canMove = true;

    bool _iFramed;

    // Start is called before the first frame update
    void OnEnable()
    {
        this._impState = EStateEnemy.PATROL;
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
        if (this._impState == EStateEnemy.ATTACK)
        {
            CheckAttack(Detect());

            if (!_isAttacking)
            {
                Move();
            }
        }
        else if (this._impState == EStateEnemy.PATROL)
        {
            CheckForPlayer(Detect());
            Move();
        }
    }

    void Move()
    {
        if (_canMove && this._impState == EStateEnemy.ATTACK)
        {
            MaintainDistanceFromTarget();
        }
        else if (_canMove && this._impState == EStateEnemy.PATROL)
        {
            // Vector2 pos = new(transform.position.x + (this.flipDirection * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
            _rb.velocity = new(Mathf.Lerp(0, _flipDirection * _speed, 0.5f), _rb.velocity.y);
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
        if (this._impState == EStateEnemy.PATROL)
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
                _impProjectile,
                new Vector3(transform.position.x + 1f * GetPlayerDirection(), transform.position.y, transform.position.z),
                Quaternion.identity);

            // slash owner
            var temp = projectile.GetComponent<HorizontalProjectile>();
            temp.SourcePlayer = gameObject;

            // flip projectile based on player face direction
            Vector3 projectileScale = projectile.transform.localScale;
            projectileScale.y *= -GetPlayerDirection();
            projectile.transform.localScale = projectileScale;

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

    override public void Hit(GameObject player, Vector2 dmgSourcePos, int damageTaken = 0)
    {

        if (!_iFramed)
        {
            if (this._impState != EStateEnemy.ATTACK)
            {
                _target = player;
                this._impState = EStateEnemy.ATTACK;
            }
            Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, 0);
            vec.Normalize();
            _rb.velocity = Vector2.zero;
            _rb.AddForce(vec * 5, ForceMode2D.Impulse);

            if (damageTaken > 0)
                Damage(damageTaken);

            StartCoroutine(Stagger());

            // Debug.Log("Imp Hit");
        }
    }

    override protected void Flip()
    {
        float flip;
        if (this._impState == EStateEnemy.ATTACK && _target != null)
        {
            flip = GetPlayerDirection();
        }

        else if (this._impState == EStateEnemy.PATROL)
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


        hits = Physics2D.BoxCastAll(transform.position, new Vector2(10f, 1f), 0, -transform.right * flip, 2.5f);


        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _target = hit.collider.gameObject;
                this._impState = EStateEnemy.ATTACK;
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
                // Debug.Log("Imp detects player");
                // Debug.Log("Imp can attack: " + canAttack);
                // Debug.Log("Imp is grounded: " + IsGrounded());
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

        Gizmos.DrawWireCube(transform.position + (2.5f * flip * -transform.right), new Vector2(10f, 1f));
    }
}
