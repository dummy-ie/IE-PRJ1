using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    CharacterController2D controller;


    private bool isAttacking = false;
    //private float attackDuration = 0.1f;
    private float attackTime;
    private float attackCooldown = .2f;

    private bool canAttack = true;

    [SerializeField]
    SpriteRenderer attackHitboxDebug;
    [SerializeField]
    SpriteRenderer attackHitboxVDebug;
    [SerializeField]
    Collider2D attackHitbox;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPressAttack();
        }
    }

    private void OnPressAttack()
    {
        if (canAttack)
        {
            canAttack = false;

            int flip = 1;
            if(controller.IsFacingRight) flip = -1;

            RaycastHit2D[] hits;

            if (controller.Vertical >= .9f)
            {
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, transform.up, 2);
            }
            else
            {
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, -transform.right * flip, 2);
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Breakable")
                {
                    hit.collider.gameObject.GetComponent<EnemyBaseScript>().Hit(hit.point);
                }
            }



            attackTime = attackCooldown;
        }
    }

    private void AttackUpdate()
    {
        if (attackTime > 0 && canAttack)
        {
            if (controller.Vertical >= .9) attackHitboxVDebug.enabled = true;
            else attackHitboxDebug.enabled = true;
            attackTime -= Time.deltaTime;
        }
        else
        {
            attackHitboxDebug.enabled = false;
            attackHitboxVDebug.enabled = false;
            canAttack = true;
        }
    }

    /*public void HitDetected(EnemyBaseScript enemy)
    {
        if (enemy.gameObject.tag == "Breakable")
        {
            enemy.Hit();
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackUpdate();
    }

    
}
