    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    //private PlayerAction pa;
    //public InputAction ia;
    //[SerializeField] private InputActionAsset controls;
    //public InputActionMap iaa;

    private Rigidbody2D rb;
    // private bool isGrounded;
    private uint jumpsLeft;
    private float horizontal;
    private float coyoteTimeCounter;
    private bool isFacingRight;
    private bool isDashing;
    private float dashDuration;
    private float dashSpeed;


    [SerializeField] LayerMask groundLayer;

    [Header("Movement")]
    [Range(0, 100)][SerializeField] private float speed, jumpForce;
    [SerializeField] private uint jumpLimit = 1;
    // [SerializeField] private float jumpForce;
    [Range(0, 10)][SerializeField] private float fallMultiplier, lowJumpMultiplier;
    [Range(0, 5)][SerializeField] private float coyoteTime, boxCastDistance;
    [SerializeField] Vector2 boxSize;

    public void onMove(InputAction.CallbackContext context)
    { 
        if (context.started)
            Debug.Log("Moving");
        else if (context.performed)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
        else if (context.canceled)
        {
            horizontal = 0;
        }
    }

    public void onDash(InputAction.CallbackContext context)
    {
        if (context.started)
         {
            dashSpeed = 30;

            dashDuration = .2f;
            isDashing = true;

            if (!isFacingRight)
                dashSpeed *= -1;
        }
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");

        Jump();
        Flip();
        
        // processInput();
    }

    private void FixedUpdate()
    {
        if(!isDashing)rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Dash();
    }

    void ProcessInput()
    {

    }

    void Move()
    {
        // float moveBy = x * speed;
        // rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    void Jump()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0))
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //     additionalJumps--;
        // }

        //if (IsGrounded())
        //{
        //    coyoteTimeCounter = coyoteTime;
        //}
        //else
        //{
        //    coyoteTimeCounter -= Time.deltaTime;
        //}

        //if (Input.GetKeyDown("Jump") && coyoteTimeCounter > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        //if (Input.GetKeyUp("Jump") && rb.velocity.y > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        //    coyoteTimeCounter = 0f;
        //}
    }

    void BetterJump()
    {
        //if (rb.velocity.y < 0)
        //{
        //    rb.velocity += (fallMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        //}
        //else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        //{
        //    rb.velocity += (lowJumpMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        //}
    }

    private void Dash()
    {
        if (isDashing)
        {
            

            dashDuration -= Time.deltaTime;

            rb.velocity = new Vector2(dashSpeed, 0); 
        }

        if (dashDuration <= 0)
        {
           // dashSpeed = 0;
            isDashing=false;
        }

        
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, boxCastDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * boxCastDistance, boxSize);
    }

    // void CheckIfGrounded()
    // {
    //     Physics.Raycast();
    //     Physics.Cas
    //     Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
    //     if (colliders != null)
    //     {
    //         isGrounded = true;
    //         additionalJumps = defaultAdditionalJumps;
    //     }
    //     else
    //     {
    //         if (isGrounded)
    //         {
    //             lastTimeGrounded = Time.time;
    //         }
    //         isGrounded = false;
    //     }
    // }
}
