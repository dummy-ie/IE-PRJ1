using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SlimeMovement : EnemyBaseScript
{

    GameObject target;

    float speed = 4;

    bool isAttackPhase = false;

    bool isAttacking = false;
    bool canAttack = true;
    bool canMove = true;

    bool iFramed;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();
        if (isAttackPhase)
        {

            CheckAttack(Detect());

            if (!isAttacking)
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
        if (canMove)
        {
            Vector2 pos = new(transform.position.x + (GetPlayerDirection() * speed) , .5f);
            transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
    }

    IEnumerator AttackLunge()
    {
        isAttacking = true;

        yield return new WaitForSeconds(.2f);

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            Vector2 vec = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
            vec.Normalize();
            vec /= new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));

            rb.AddForce(new Vector2(vec.x * 5, 3f), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1);

        canAttack = true;
        isAttacking = false;

    }

    IEnumerator Stagger()
    {
        canAttack = false;
        canMove = false;
        isAttacking = false;
        yield return new WaitForSeconds(.7f);
        canAttack = true;
        canMove = true;
    }

    override public void Hit(Vector2 dmgSourcePos)
    {
        if (!iFramed)
        {
            
            Vector2 vec = new Vector2(transform.position.x - target.transform.position.x, 0);
            vec.Normalize();
            rb.velocity = Vector2.zero;
            rb.AddForce(vec * 5, ForceMode2D.Impulse);


            StartCoroutine(Stagger());

            Debug.Log("Slime Hit");
        }
    }

    override protected void Flip()
    {
        float flip;
        if (isAttackPhase && target != null)
        {
            flip = GetPlayerDirection();
        }
        else
        {
            flip = rb.velocity.x;
        }
        if (isFacingRight && flip < 0f || !isFacingRight && flip > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private float GetPlayerDirection()
    {
        float value = target.transform.position.x - transform.position.x;
        if (value < 0)
            return -1;
        else
            return 1;
    }

    RaycastHit2D[] Detect()
    {      
        int flip = 1;
        if (isFacingRight) flip = -1;
        RaycastHit2D[] hits;

        
        hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, 1f), 0, -transform.right * flip, 2.5f);
        

        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                target = hit.collider.gameObject;
                isAttackPhase = true;
            }
        }
    }

    void CheckAttack(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (canAttack && IsGrounded())
                {
                    StartCoroutine(AttackLunge());
                    canAttack = false;
                }
            }
        }
    }

}
