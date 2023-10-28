using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ManiteSlash : MonoBehaviour
{

    CharacterController2D controller;

    private bool isAttacking = false;
    // private bool isAttacking = false;
    private float attackTime;
    private float attackCooldown = 1.5f;

    private bool canAttack = true;

    [SerializeField]
    private GameObject slashProjectile = null;
    
    [SerializeField]
    [Range(0, 5)]
    private int slashSpawnDistance = 1;

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
                new Vector3(controller.transform.position.x + slashSpawnDistance * -flip, controller.transform.position.y, controller.transform.position.z),
                Quaternion.identity);

            // slash owner
            var temp = projectile.GetComponent<ManiteSlashProjectile>();
            temp.SourcePlayer = gameObject;

            // rotate dat bitch
            projectile.transform.Rotate(new Vector3(0f, 0f, -90f));

            // flip projectile based on player face direction
            Vector3 projectileScale = projectile.transform.localScale;
            projectileScale.y *= flip;
            projectile.transform.localScale = projectileScale;

            StartCoroutine(VecShift());
            StartCoroutine(Cooldown());

            attackTime = attackCooldown;
        }
    }

    private void ManiteSlashUpdate()
    {
        /*if (isAttacking)
        {
            attackTime -= Time.deltaTime;
        }

        if (attackTime <= 0)
        {
            isAttacking = false;
        }*/
    }

    IEnumerator VecShift() //temp
    {
        controller.ShiftTo3D();
        controller.RB.drag = 100f;

        yield return new WaitForSeconds(.7f);

        controller.ShiftTo2D();
        controller.RB.drag = 0;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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

