using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ImpMovement : EnemyBaseScript
{
    private GameObject target;

    private float speed = 4;

    private float flipDirection = 1;

    [SerializeField]
    private EStateEnemy impState;

    [SerializeField]
    private float distanceFromTarget = 5f;

    [SerializeField]
    private GameObject impProjectile;

    public EStateEnemy ImpState
    {
        get { return this.impState; }
        set { this.impState = value; }
    }

    bool isAttacking = false;
    bool canAttack = true;
    bool canMove = true;

    bool iFramed;

    // Start is called before the first frame update
    void OnEnable()
    {
        this.impState = EStateEnemy.PATROL;
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
        if (this.impState == EStateEnemy.ATTACK)
        {
            CheckAttack(Detect());

            if (!isAttacking)
            {
                Move();
            }
        }
        else if (this.impState == EStateEnemy.PATROL)
        {
            CheckForPlayer(Detect());
            Move();
        }
    }

    void Move()
    {
        if (canMove && this.impState == EStateEnemy.ATTACK)
        {
            MaintainDistanceFromTarget();
        }
        else if (canMove && this.impState == EStateEnemy.PATROL)
        {
            // Vector2 pos = new(transform.position.x + (this.flipDirection * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
            rb.velocity = new(Mathf.Lerp(0, flipDirection * speed, 0.5f), rb.velocity.y);
        }
    }

    void MaintainDistanceFromTarget()
    {
        float calculatedDistance = Mathf.Abs(Vector2.Distance(target.transform.position, transform.position));
        // Debug.Log("Distance from target: " + calculatedDistance);
        if (calculatedDistance < distanceFromTarget - 0.5f)
        {
            rb.velocity = new(Mathf.Lerp(0, -GetPlayerDirection() * speed, 0.5f), rb.velocity.y);
            // Vector2 pos = new(transform.position.x - (this.GetPlayerDirection() * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
        else if (calculatedDistance > distanceFromTarget + 0.5f)
        {
            rb.velocity = new(Mathf.Lerp(0, GetPlayerDirection() * speed, 0.5f), rb.velocity.y);
            // Vector2 pos = new(transform.position.x + (this.GetPlayerDirection() * speed), 0);
            // transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
    }

    IEnumerator FlipInterval()
    {
        if (this.impState == EStateEnemy.PATROL)
        {

            this.flipDirection = UnityEngine.Random.Range(0, 2);

            if (this.flipDirection == 0)
            {
                this.flipDirection = -1;
            }

        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(FlipInterval());



    }

    IEnumerator AttackShoot()
    {
        isAttacking = true;

        yield return new WaitForSeconds(.2f);

        if (isAttacking)
        {
            GameObject projectile = Instantiate(
                impProjectile,
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

        isAttacking = false;

        yield return new WaitForSeconds(1);

        canAttack = true;

    }

    IEnumerator Stagger()
    {
        iFramed = true;
        canAttack = canMove = isAttacking = false;
        StopCoroutine(AttackShoot());

        yield return new WaitForSeconds(.7f);

        iFramed = false;
        canMove = true;

        yield return new WaitForSeconds(.7f);

        canAttack = true;
    }

    override public void Hit(GameObject player, Vector2 dmgSourcePos, int damageTaken = 0)
    {

        if (!iFramed)
        {
            if (this.impState != EStateEnemy.ATTACK)
            {
                target = player;
                this.impState = EStateEnemy.ATTACK;
            }
            Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, 0);
            vec.Normalize();
            rb.velocity = Vector2.zero;
            rb.AddForce(vec * 5, ForceMode2D.Impulse);

            if (damageTaken > 0)
                Damage(damageTaken);

            StartCoroutine(Stagger());

            // Debug.Log("Imp Hit");
        }
    }

    override protected void Flip()
    {
        float flip;
        if (this.impState == EStateEnemy.ATTACK && target != null)
        {
            flip = GetPlayerDirection();
        }

        else if (this.impState == EStateEnemy.PATROL)
        {
            flip = this.flipDirection;
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


        hits = Physics2D.BoxCastAll(transform.position, new Vector2(10f, 1f), 0, -transform.right * flip, 2.5f);


        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                target = hit.collider.gameObject;
                this.impState = EStateEnemy.ATTACK;
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
                if (canAttack && IsGrounded())

                {
                    StartCoroutine(AttackShoot());
                    canAttack = false;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        int flip = 1;
        if (isFacingRight) flip = -1;

        Gizmos.DrawWireCube(transform.position + (2.5f * flip * -transform.right), new Vector2(10f, 1f));
    }
}
