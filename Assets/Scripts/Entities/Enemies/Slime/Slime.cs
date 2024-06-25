using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SlimeBehaviour : EnemyBase<SlimeBehaviour>, IEntityHittable
{

    private GameObject _target;

    private float _speed = 4;

    private float _flipDirection = 1;

    bool _isAttacking = false;
    bool _canAttack = true;
    bool _canMove = true;

    bool _iFramed;

    public void OnHit(HitData hitData)
    {
        /*if (!_iFramed)
        {
            if (this._slimeState != EStateEnemy.ATTACK)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
                this._slimeState = EStateEnemy.ATTACK;
            }
            Vector2 vec = new Vector2(transform.position.x - source.position.x, 0);
            vec.Normalize();
            _rb.velocity = Vector2.zero;
            _rb.AddForce(vec * 5, ForceMode2D.Impulse);

            if (damage > 0)
                Damage(damage);


            StartCoroutine(Stagger());
            StartCoroutine(Blink());
            Debug.Log("Slime Hit");
        }*/
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //this._slimeState = EStateEnemy.PATROL;
        //this.StartCoroutine(FlipInterval());

    }

    void Start()
    {
        //this.StartCoroutine(FlipInterval());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Flip();
        /*if (this._slimeState == EStateEnemy.ATTACK)
        {

            CheckAttack(Detect());

            if (!_isAttacking)
            {
                Move();
            }


        }
        else if(this._slimeState == EStateEnemy.PATROL)
        {
            CheckForPlayer(Detect());
            Move();
        }
        */

    }

    void Move()
    {
        /*if (_canMove && this._slimeState == EStateEnemy.ATTACK)
        {
            Vector2 pos = new(transform.position.x + (this.GetPlayerDirection() * _speed), .5f);
            transform.position = Vector2.MoveTowards(_rb.transform.position, pos, Time.deltaTime * _speed);
        }

        else if(_canMove && this._slimeState == EStateEnemy.PATROL){

            Vector2 pos = new(transform.position.x + (this._flipDirection * _speed), .5f);
            transform.position = Vector2.MoveTowards(_rb.transform.position, pos, Time.deltaTime * _speed);
        }
    }

    IEnumerator FlipInterval(){


        if(this._slimeState == EStateEnemy.PATROL){

            this._flipDirection = Random.Range(0,2);

            if(this._flipDirection == 0){
                this._flipDirection = -1;
            }

        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(FlipInterval());
    }

    IEnumerator AttackLunge()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(.2f);

        if (_isAttacking)
        {
            _rb.velocity = Vector2.zero;
            Vector2 vec = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.y - transform.position.y);
            vec.Normalize();
            vec /= new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));

            _rb.AddForce(new Vector2(vec.x * 5, 3f), ForceMode2D.Impulse);
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
        StopCoroutine(AttackLunge());

        yield return new WaitForSeconds(.7f);

        _iFramed = false;
        _canMove = true;

        yield return new WaitForSeconds(.7f);

        _canAttack = true;
    }


    override protected void Flip()
    {
        float flip;
        if (this._slimeState == EStateEnemy.ATTACK && _target != null)
        {
            flip = GetPlayerDirection();
        }

        else if (this._slimeState == EStateEnemy.PATROL){
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


        hits = Physics2D.BoxCastAll(transform.position, new Vector2(1.5f, 1f), 0, -transform.right * flip, 2.5f);


        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _target = hit.collider.gameObject;
                this._slimeState = EStateEnemy.ATTACK;
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
                if (_canAttack && IsGrounded())
                {
                    StartCoroutine(AttackLunge());
                    _canAttack = false;
                }
            }
        }
    }
        */
    }
}
