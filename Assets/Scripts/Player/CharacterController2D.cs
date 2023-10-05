using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private uint jumpsLeft = 0;
    private float horizontal;
    private float coyoteTimeCounter = 0f, jumpBufferCounter = 0f;
    private bool isFacingRight = false;
    private bool isDashing = false;
    private bool isJumpPress = false;
    private float dashTime = 0f;
    private float dashSpeed = 0f;

    [SerializeField] LayerMask groundLayer;

    // have default values for all fields to prevent null errors
    [Header("Movement")]
    [Range(0, 100)][SerializeField] private float speed = 6f;

    [Header("Jumping")]
    [Range(0, 100)][SerializeField] private float jumpForce = 14f;
    [SerializeField] private uint jumpLimit = 1;
    [Range(0, 10)][SerializeField] private float fallMultiplier = 4f, lowJumpMultiplier = 1f;
    [Range(0, 5)][SerializeField] private float coyoteTime = 0.2f, jumpBufferTime = 0.2f;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float boxCastDistance = 0.4f;
    [SerializeField] Vector2 boxSize = new(0.3f, 0.4f);

    [Header("Dashing")]
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashOriginalSpeed = 30f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = jumpLimit;
    }

    private void Update()
    {
        Jump();
        Dash();
        Flip();
    }

    private void FixedUpdate() // move player on fixed update so collisions aren't fucky wucky
    {
        if (!isDashing) rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // reset jump buffer counter
            jumpBufferCounter = jumpBufferTime;
            isJumpPress = true;
        }
        else if (context.canceled)
        {
            isJumpPress = false;
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                coyoteTimeCounter = 0f;
            }
        }
    }


    private void Jump()
    {
        // while grounded, set coyote time and jump limit
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            jumpsLeft = jumpLimit;
        }
        else // otherwise, count down the coyote time
            coyoteTimeCounter -= Time.deltaTime;

        if (!isJumpPress) jumpBufferCounter -= Time.deltaTime; // when not pressing jump, count down jump buffering time

        // jump while jumps available and jump is buffered and coyote time still active 
        if (jumpsLeft > 0f && jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            // Debug.Log("Jumping!");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            // decrement jumps
            if (jumpsLeft > 0f)
                jumpsLeft--;
        }

        if (rb.velocity.y < 0) // fast fall
        {
            rb.velocity += (fallMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
        else if (rb.velocity.y > 0 && !isJumpPress) // low jump
        {
            rb.velocity += (lowJumpMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
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
