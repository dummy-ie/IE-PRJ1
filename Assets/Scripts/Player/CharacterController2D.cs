using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private uint jumpsLeft;
    private float horizontal;
    private float coyoteTimeCounter, jumpBufferCounter;
    private bool isFacingRight;
    private bool isDashing;
    private bool isJumping;
    private float dashTime;
    private float dashSpeed;

    [SerializeField] LayerMask groundLayer;

    [Header("Movement")]
    [Range(0, 100)][SerializeField] private float speed, jumpForce;
    [SerializeField] private uint jumpLimit = 1;
    [Range(0, 10)][SerializeField] private float fallMultiplier, lowJumpMultiplier;
    [Range(0, 5)][SerializeField] private float coyoteTime, jumpBufferTime, boxCastDistance;
    [SerializeField] Vector2 boxSize;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashOriginalSpeed;

    public void OnMove(InputAction.CallbackContext context)
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dashSpeed = dashOriginalSpeed;
            isDashing = true;

            if (!isFacingRight)
                dashSpeed *= -1;
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isJumping = context.started)
        {
            // start jump buffer counter
            jumpBufferCounter = jumpBufferTime;

            Debug.Log("Jump Buffer Counter Before Jump: " + jumpBufferCounter.ToString());
            // Debug.Log("Coyote Counter Before Jump: " + coyoteTimeCounter.ToString());
            // Debug.Log("Jumps Left Before Jump: " + jumpsLeft.ToString());

            // jump while coyote time available and jumps available
            if (jumpsLeft > 0f && coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                Debug.Log("Jumping!");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpBufferCounter = 0f;
            }

            // decrement jumps
            if (jumpsLeft > 0f)
                jumpsLeft--;

            // Debug.Log("Jumps Left After Jump: " + jumpsLeft.ToString());

        }
        else if (context.performed)
        {
        }
        else if (context.canceled)
        {
            isJumping = false;
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                coyoteTimeCounter = 0f;
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = jumpLimit;
        jumpBufferCounter = jumpBufferTime;
    }

    private void FixedUpdate()
    {
        Flip();
        BetterJump();

        if (IsGrounded())
        {
            // Debug.Log("Grounded"); // shane you mf why would you print this every frame!!!
            coyoteTimeCounter = coyoteTime;
            jumpsLeft = jumpLimit;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (!isJumping) jumpBufferCounter -= Time.deltaTime;

        if (!isDashing) rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Dash();
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
            dashTime -= Time.deltaTime;

            rb.velocity = new Vector2(dashSpeed, 0);
        }

        if (dashTime <= 0)
        {
            dashTime = dashDuration;
            isDashing = false;
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
}
