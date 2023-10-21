using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ManiteSlash : MonoBehaviour
{

    CharacterController2D controller;

    // private bool isAttacking = false;
    private float attackTime;
    private float attackCooldown = .2f;

    private bool canAttack = true;

    [SerializeField]
    private GameObject slashProjectile = null;
    
    [SerializeField]
    [Range(0, 5)]
    private int slashDistance = 1;

    public void OnManiteSlash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("manite slash");
            OnPressManiteSlash();
        }
    }

    private void OnPressManiteSlash()
    {
        if (canAttack)
        {
            canAttack = false;

            int flip = 1;
            if (controller.IsFacingRight) flip = -1;

            GameObject projectile = Instantiate(
                slashProjectile,
                new Vector3(controller.transform.position.x + slashDistance * -flip, controller.transform.position.y, controller.transform.position.z),
                Quaternion.identity);

            // rotate dat bitch
            projectile.transform.Rotate(new Vector3(0f, 0f, -90f));

            // flip projectile based on player face direction
            Vector3 projectileScale = projectile.transform.localScale;
            projectileScale.y *= flip;
            projectile.transform.localScale = projectileScale;

            attackTime = attackCooldown;
        }
    }

    private void ManiteSlashUpdate()
    {
        if (attackTime > 0 && canAttack)
        {
            attackTime -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        if (slashProjectile == null)
            Debug.LogError("ManiteSlash.cs: Assign a slash projectile!");
    }

    private void Update()
    {
        ManiteSlashUpdate();
    }
}

