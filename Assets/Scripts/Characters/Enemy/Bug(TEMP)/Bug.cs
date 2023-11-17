using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BugMovement : EnemyBaseScript
{

    GameObject _target;

    float _speed = 4;

    
    bool _isAttackPhase = false;

    bool _isAttacking = false;
    bool _canAttack = true;
    bool _canMove = true;

    bool _iFramed;

    Vector3 _velocity;

    override public void OnHit(Transform source, int damage)
    {

        if (!_iFramed)
        {
            if (!_isAttackPhase)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
                _isAttackPhase = true;
            }
            Vector2 vec = new Vector2(_rb.transform.position.x - source.position.x, _rb.transform.position.y - source.position.y);
            vec.Normalize();
            _rb.velocity = Vector2.zero;
            _rb.AddForce(vec * 5, ForceMode2D.Impulse);


            StartCoroutine(Stagger());

            Debug.Log("BUG Hit");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    { 

        Flip();
        if (_isAttackPhase)
        {

            CheckAttack(Detect());

            if (!_isAttacking)
            {
                Move();
            }

            
        }
        else
        {
            CheckForPlayer(Detect());
        }


    }

    void Move()
    {
        if (_canMove)
        {
            _rb.transform.position = Vector3.SmoothDamp(_rb.transform.position, _target.transform.position, ref _velocity, 1f, _speed);
        }
    }

    IEnumerator AttackLunge()
    {
        _isAttacking = true;

        Vector2 vec = new Vector2(_target.transform.position.x - _rb.transform.position.x, _target.transform.position.y - _rb.transform.position.y);

        yield return new WaitForSeconds(.2f);

        if (_isAttacking)
        {
            _rb.velocity = Vector2.zero;
            
            vec.Normalize();

            _rb.AddForce(vec * 8, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(.3f);

        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1);

        _isAttacking = false;

        yield return new WaitForSeconds(2);

        _canAttack = true;

    }

    IEnumerator Stagger()
    {
        _iFramed = true;
        _canAttack = _canMove = _isAttacking = false;
        StopCoroutine(AttackLunge());
        
        yield return new WaitForSeconds(.7f);

        _rb.velocity = Vector3.zero;
        _canMove = true;
        _iFramed = false;

        yield return new WaitForSeconds(.7f);

        _canAttack = true;
    }


    override protected void Flip()
    {
        float flip;
        if (_isAttackPhase && _target != null)
        {
            flip = GetPlayerDirection();
        }
        else
        {
            flip = _rb.velocity.x;
        }
        if (_isFacingRight && flip < 0f || !_isFacingRight && flip > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = _rb.transform.localScale;
            localScale.x *= -1f;
            _rb.transform.localScale = localScale;
        }
    }

    private float GetPlayerDirection()
    {
        float value = _target.transform.position.x - _rb.transform.position.x;
        if (value < 0)
            return -1;
        else
            return 1;
    }

    RaycastHit2D[] Detect()
    {      
        RaycastHit2D[] hits;

        
        hits = Physics2D.CircleCastAll(_rb.transform.position, 1.5f, Vector2.zero);
        

        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _target = hit.collider.gameObject;
                _isAttackPhase = true;
            }
        }
    }

    void CheckAttack(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (_canAttack)
                {
                    StartCoroutine(AttackLunge());
                    _canAttack = false;
                }
            }
        }
    }

}
