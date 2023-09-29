    using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    //public InputAction ia;
    //[SerializeField] private InputActionAsset controls;
    //public InputActionMap iaa;

    private PlayerInput pi;
    private Rigidbody2D rb;
    // private bool isGrounded;
    private uint jumpsLeft;
    private float horizontal;
    private float coyoteTimeCounter;
    private bool isFacingRight;

    private bool isJumping;

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
        float moveX = context.ReadValue<Vector2>().x;
        if (context.started)
        {
            Debug.Log("Move pressed");
        }
        else if (context.performed)
        {
            Debug.Log("Moving now" + moveX);

            if (moveX >= 0.5f || moveX <= -0.5f)
                horizontal = moveX;
        }
        else if (context.canceled)
        {
            Debug.Log("Move released");
            horizontal = 0;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (isJumping = context.started)
        {
            if (coyoteTimeCounter > 0f || jumpsLeft > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft--;
            }
        }
        else if (context.performed)
        {
        }
        else if (isJumping = context.canceled)
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                coyoteTimeCounter = 0f;
            }
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
        pi = GetComponent<PlayerInput>();

        jumpsLeft = jumpLimit;
        //playerAction.Player.Jump.performed;
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
        Flip();

        BetterJump();

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            jumpsLeft = jumpLimit;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        
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

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
        else if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += (lowJumpMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
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
