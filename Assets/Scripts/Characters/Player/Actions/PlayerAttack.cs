using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    CharacterController2D controller;

    [SerializeField]
    private PlayerAttackData _attackData;
    public PlayerAttackData AttackData
    {
        get { return _attackData; }
        set { _attackData = value; }
    }

    

    private bool isAttacking = false;
    //private float attackDuration = 0.1f;
    private float attackTime;

    //[SerializeField]
    //private bool canAttack = true;

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
        if (controller.Data.CanAttack)
        {
            isAttacking = true;
            controller.Data.CanAttack = false;

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
                // Debug.Log($"Hit : {hit.collider.gameObject.name}");
                /*if (hit.collider.gameObject.CompareTag("Breakable"))
                {
                    EnemyBaseScript enemy;
                    enemy = hit.collider.gameObject.GetComponent<EnemyBaseScript>();

                    if (enemy != null)
                    {
                        enemy.Hit(gameObject, gameObject.transform.position, playerAttackDamage);
                    }
                }

                else if(hit.collider.gameObject.CompareTag("EnvBreakable"))
                {
                    BreakEnvironment environment;
                    environment = hit.collider.gameObject.GetComponent<BreakEnvironment>();

                    if (environment != null){
                        environment.Hit();
                    }


                }*/
                if (hit.collider.gameObject.layer == 6)
                    break;
                IHittable handler = hit.collider.gameObject.GetComponent<IHittable>();
                if (handler != null)
                {
                    handler.OnHit(transform, _attackData.NormalAttackDamage);
                }
            }

            /*RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 0.5f);
            if (hit.collider != null) {
                Debug.Log($"hit: {hit.collider.gameObject.name}");
                IHittable handler = hit.collider.gameObject.GetComponent<IHittable>();
                if (handler != null) {
                    //if (hit.collider.gameObject.GetComponent<BreakableWall>())
                        handler.OnHit(playerAttackDamage);
                }
            }*/

            TriggerCooldown();

            attackTime = _attackData.NormalAttackCooldown;
        }
    }

    private void AttackUpdate()
    {
        if (isAttacking)
        {
            if (controller.Vertical >= .9) attackHitboxVDebug.enabled = true;
            else attackHitboxDebug.enabled = true;
            attackTime -= Time.deltaTime;
        }

        if (attackTime <= 0)
        {
            attackHitboxDebug.enabled = false;
            attackHitboxVDebug.enabled = false;
            isAttacking = false;
        }
    }

    void TriggerCooldown()
    {
        controller.Data.AttackCooldown = _attackData.NormalAttackCooldown;
        StartCoroutine(controller.Cooldown());
    }

    /*public void HitDetected(EnemyBaseScript enemy)
    {
        if (enemy.gameObject.tag == "Breakable")
        {
            enemy.Hit();
        }
    }*/

    IEnumerator VecShift() //temp
    {
        controller.ShiftTo3D();

        yield return new WaitForSeconds(1);

        controller.ShiftTo2D();
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(.5f, .5f));
    }


}
