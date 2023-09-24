using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAction PlayerAction;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private float lastTimeGrounded;
    private bool onGround;
    private int additionalJumps;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float moveSpeed, jumpSpeed;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float rememberGroundedFor;
    [SerializeField] private int defaultAdditionalJumps;

    private void Awake()
    {
        PlayerAction = new PlayerAction();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        PlayerAction.Enable();
    }

    private void OnDisable()
    {
        PlayerAction.Disable();
    }

    void Start()
    {
        PlayerAction.Ground.Jump.performed += jumpCtx => Jump(jumpCtx.ReadValue<float>());
    }

    void FixedUpdate()
    {
        //Read 
        float movementInput = PlayerAction.Ground.Move.ReadValue<float>();
        //Move
        float moveBy = movementInput * moveSpeed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
        //BetterJump
        rememberGround();
        betterJump();
    }

    private void Jump(float val)
    {
        if (val==1 && onGround)
        {
            //rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            additionalJumps--;
        }else if (val == 1 && (Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }else if (val == 1 && additionalJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            additionalJumps--;
        }
    }

    private void rememberGround()
    {
        if (col.IsTouchingLayers(Ground))
        {
            onGround = true;
            additionalJumps = defaultAdditionalJumps;
        }
        else
        {
            if (onGround)
            {
                lastTimeGrounded = Time.time;
            }
            onGround = false;
        }
    }

    void betterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
        else if (rb.velocity.y > 0 && PlayerAction.Ground.Jump.ReadValue<float>() == 0)
        {
            rb.velocity += (lowJumpMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
    }

    //old function to check if grounded, replaced this with col.IsTouchingLayers(Ground)
    //private bool IsGrounded()
    //{
    //    Vector2 topLeft = transform.position;
    //    topLeft.x -= col.bounds.extents.x;
    //    topLeft.y += col.bounds.extents.y;

    //    Vector2 botRight = transform.position;
    //    botRight.x += col.bounds.extents.x;
    //    botRight.y -= col.bounds.extents.y;

    //    return Physics2D.OverlapArea(topLeft, botRight, Ground);
    //}
}
