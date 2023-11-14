using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BugMovement : EnemyBaseScript
{

    GameObject target;

    float speed = 4;

    bool isAttackPhase = false;

    bool isAttacking = false;
    bool canAttack = true;
    bool canMove = true;

    bool iFramed;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
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
            rb.transform.position = Vector3.SmoothDamp(rb.transform.position, target.transform.position, ref velocity, 1f, speed);
        }
    }

    IEnumerator AttackLunge()
    {
        isAttacking = true;

        Vector2 vec = new Vector2(target.transform.position.x - rb.transform.position.x, target.transform.position.y - rb.transform.position.y);

        yield return new WaitForSeconds(.2f);

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            
            vec.Normalize();

            rb.AddForce(vec * 8, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(.3f);

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1);

        isAttacking = false;

        yield return new WaitForSeconds(2);

        canAttack = true;

    }

    IEnumerator Stagger()
    {
        iFramed = true;
        canAttack = canMove = isAttacking = false;
        StopCoroutine(AttackLunge());
        
        yield return new WaitForSeconds(.7f);

        rb.velocity = Vector3.zero;
        canMove = true;
        iFramed = false;

        yield return new WaitForSeconds(.7f);

        canAttack = true;
    }

    override public void Hit(GameObject player, Vector2 dmgSourcePos, int damageTaken = 0)
    {
        
        if (!iFramed)
        {
            if (!isAttackPhase)
            {
                target = player;
                isAttackPhase = true;
            }
            Vector2 vec = new Vector2(rb.transform.position.x - dmgSourcePos.x, rb.transform.position.y - dmgSourcePos.y);
            vec.Normalize();
            rb.velocity = Vector2.zero;
            rb.AddForce(vec * 5, ForceMode2D.Impulse);


            StartCoroutine(Stagger());

            Debug.Log("BUG Hit");
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
            Vector3 localScale = rb.transform.localScale;
            localScale.x *= -1f;
            rb.transform.localScale = localScale;
        }
    }

    private float GetPlayerDirection()
    {
        float value = target.transform.position.x - rb.transform.position.x;
        if (value < 0)
            return -1;
        else
            return 1;
    }

    RaycastHit2D[] Detect()
    {      
        RaycastHit2D[] hits;

        
        hits = Physics2D.CircleCastAll(rb.transform.position, 1.5f, Vector2.zero);
        

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
                if (canAttack)
                {
                    StartCoroutine(AttackLunge());
                    canAttack = false;
                }
            }
        }
    }

}
