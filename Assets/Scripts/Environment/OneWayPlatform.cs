using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlatformEffector2D))]
public class OneWayPlatform : MonoBehaviour
{

    private bool _isColliding = false;
    private bool _isPressDown = false;
    private bool _isPressJump = false;
    private PlatformEffector2D _platform;

    public void OnPressDown(InputAction.CallbackContext context)
    {
        float moveY = context.ReadValue<Vector2>().y;
        if (context.started)
        {
        }
        else if (context.performed)
        {
            if (moveY <= -0.5f)
                _isPressDown = true;

        }
        else if (context.canceled)
        {
            _isPressDown = false;
        }
    }

    public void OnPressJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressJump = true;
        }
        else if (context.canceled)
        {
            _isPressJump = false;
        }
    }

    void Start()
    {
        _platform = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (_isColliding && _isPressDown && _isPressJump)
        {
            Debug.Log("SHOULD DROP DOWN");
            _platform.surfaceArc = 0f;
            StartCoroutine(Wait());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLIDING WITH PLATFORM");
        _isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isColliding = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        _platform.surfaceArc = 180f;
    }
}

