using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    private bool facingRight = true;
    private PlayerAction PlayerAction;
    private void Awake()
    {
        PlayerAction = new PlayerAction();
    }
    void Start()
    {
        PlayerAction.Ground.Aim.performed += aimCtx => spriteFlip(aimCtx.ReadValue<Vector2>());
    }

    private void spriteFlip(Vector2 mousePos)
    {
        var dir = new Vector3(mousePos.x, mousePos.y, 0f) - Camera.main.WorldToScreenPoint(transform.position);
        var playerPos = Camera.main.WorldToScreenPoint(dir) - transform.position;
        if ((playerPos.x < 0) && facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
        else if ((playerPos.x > 0) && !facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
    }

    private void OnEnable()
    {
        PlayerAction.Enable();
    }

    private void OnDisable()
    {
        PlayerAction.Disable();
    }
}
