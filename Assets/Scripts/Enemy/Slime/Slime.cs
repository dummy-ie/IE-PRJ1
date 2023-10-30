using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SlimeMovement : EnemyBaseScript
{

    private GameObject target;

    private float speed = 4;

    private float flipDirection = 1;

    [SerializeField]
    private EStateEnemy slimeState;

    public EStateEnemy SlimeState {
        get {return this.slimeState; }
        set {this.slimeState = value; }
    }

    bool isAttacking = false;
    bool canAttack = true;
    bool canMove = true;

    bool iFramed;

    // Start is called before the first frame update
    void OnEnable()
    {
       this.slimeState = EStateEnemy.PATROL;
       this.StartCoroutine(FlipInterval());
       
    }

    void Start(){
        this.StartCoroutine(FlipInterval());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();
        if (this.slimeState == EStateEnemy.ATTACK)
        {

            CheckAttack(Detect());

            if (!isAttacking)
            {
                Move();
            }

            
        }
        else if(this.slimeState == EStateEnemy.PATROL)
        {
            CheckForPlayer(Detect());
            Move();
        }


    }

    void Move()
    {
        if (canMove && this.slimeState == EStateEnemy.ATTACK)
        {
            Vector2 pos = new(transform.position.x + (this.GetPlayerDirection() * speed), .5f);
            transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }

        else if(canMove && this.slimeState == EStateEnemy.PATROL){

            Vector2 pos = new(transform.position.x + (this.flipDirection * speed), .5f);
            transform.position = Vector2.MoveTowards(rb.transform.position, pos, Time.deltaTime * speed);
        }
    }

    IEnumerator FlipInterval(){

        Debug.Log("Direction is flipped.");
        
        if(this.slimeState == EStateEnemy.PATROL){

            this.flipDirection = Random.Range(0,2);

            if(this.flipDirection == 0){
                this.flipDirection = -1;
            }

        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(FlipInterval());

        

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

        isAttacking = false;

        yield return new WaitForSeconds(1);

        canAttack = true;

    }

    IEnumerator Stagger()
    {
        iFramed = true;
        canAttack = canMove = isAttacking = false;
        StopCoroutine(AttackLunge());

        yield return new WaitForSeconds(.7f);

        iFramed = false;
        canMove = true;

        yield return new WaitForSeconds(.7f);

        canAttack = true;
    }

    override public void Hit(GameObject player, Vector2 dmgSourcePos)
    {
        
        if (!iFramed)
        {
            if (this.slimeState != EStateEnemy.ATTACK)
            {
                target = player;
                this.slimeState = EStateEnemy.ATTACK;
            }
            Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, 0);
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
        if (this.slimeState == EStateEnemy.ATTACK && target != null)
        {
            flip = GetPlayerDirection();
        }

        else if (this.slimeState == EStateEnemy.PATROL){
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

        
        hits = Physics2D.BoxCastAll(transform.position, new Vector2(1.5f, 1f), 0, -transform.right * flip, 2.5f);
        

        return hits;
    }



    void CheckForPlayer(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                target = hit.collider.gameObject;
                this.slimeState = EStateEnemy.ATTACK;
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
                if (canAttack && IsGrounded())
                {
                    StartCoroutine(AttackLunge());
                    canAttack = false;
                }
            }
        }
    }

}
