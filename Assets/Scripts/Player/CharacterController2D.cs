using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;

    private CinemachineVirtualCamera cmVC;
    private Cinemachine3rdPersonFollow cmTP;

    
    private float horizontal = 0f;
    private float coyoteTimeCounter = 0f, jumpBufferCounter = 0f;
    
    private float vectorShift = 100f;

    private bool isFacingRight = false;

    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashSpeed = 0f;

    private bool isJumpPress = false;
    private bool extraJump = false;
    private bool isGrounded = false;



    [SerializeField] SpriteRenderer _2DRender;
    [SerializeField] MeshRenderer _3DModel;

    [SerializeField] LayerMask groundLayer;

    // have default values for all fields to prevent null errors
    [Header("Movement")]
    [Range(0, 100)][SerializeField] private float speed = 6f;

    [Header("Jumping")]
    [SerializeField] private bool allowDoubleJump = false;
    [Range(0, 100)][SerializeField] private float jumpForce = 14f;
    [Range(0, 10)][SerializeField] private float fallMultiplier = 4f, lowJumpMultiplier = 0.8f;
    [Range(0, 5)][SerializeField] private float coyoteTime = 0.2f, jumpBufferTime = 0.2f;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float boxCastDistance = 0.4f;
    [SerializeField] Vector2 boxSize = new(0.3f, 0.4f);

    [Header("Dashing")]
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashOriginalSpeed = 30f;


    

    public bool IsFacingRight {
        get { return isFacingRight; }
    }

    private void Awake()
    {
        cmVC = FindFirstObjectByType<CinemachineVirtualCamera>();
        cmTP = cmVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        rb = GetComponent<Rigidbody2D>();
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

        // interpolate the camera towards where the player is currently moving if they are moving?
        // propose the idea later on idk
        //if (horizontal != 0) cmTP.CameraSide = Mathf.Lerp(cmTP.CameraSide, 0.5f * (horizontal + 1f), 0.05f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveX = context.ReadValue<Vector2>().x;
        if (context.started)
        {
            // Debug.Log("Move pressed");
        }
        else if (context.performed)
        {
            // Debug.Log("Moving now" + moveX);

            if (moveX >= 0.5f || moveX <= -0.5f)
                horizontal = moveX;
        }
        else if (context.canceled)
        {
            // Debug.Log("Move released");
            horizontal = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPress = true;
            // reset jump buffer counter
            jumpBufferCounter = jumpBufferTime;
        }
        else if (context.canceled)
        {
            isJumpPress = false;
        }
    }

    private void Jump()
    {
        // while grounded, set coyote time and jump limit
        if (isGrounded = IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            extraJump = true; // refresh double jump
        }
        else
            coyoteTimeCounter -= Time.deltaTime; // otherwise, count down the coyote time

        if (!IsGrounded())
            jumpBufferCounter -= Time.deltaTime; // when not pressing jump, count down jump buffering time

        if (!isJumpPress && rb.velocity.y > 0f)
        {
            // fall when releasing the jump button early (allows low jumping)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * lowJumpMultiplier);
            coyoteTimeCounter = 0f;
        }
        else if (rb.velocity.y < 0f) // fast fall over time
        {
            rb.velocity += (fallMultiplier - 1f) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }

        // Jump if the buffer counter is active AND if coyote time is active OR you have an extra jump AND you can double jump
        if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || (extraJump && allowDoubleJump)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;

            if (!isGrounded && coyoteTimeCounter <= 0f)
                extraJump = false;
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

            _2DRender.enabled = false;
            _3DModel.enabled = true;
        }
        else
        {
            _2DRender.enabled = true;
            _3DModel.enabled = false;
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
