using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AimAtMouse : MonoBehaviour
{
    private PlayerAction PlayerAction;

    private void Awake()
    {
        PlayerAction = new PlayerAction();
    }

    private void Start()
    {
        PlayerAction.Ground.Aim.performed += ctx => Aim(ctx.ReadValue<Vector2>());
    }

    private void Aim(Vector2 mousePos)
    {
        var dir = new Vector3(mousePos.x, mousePos.y, 0f) - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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