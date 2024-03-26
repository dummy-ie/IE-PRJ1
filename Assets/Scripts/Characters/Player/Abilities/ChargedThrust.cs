using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ChargedThrust : AAbility
{

    // Start is called before the first frame update
    private bool isAttacking = false;
    //private float attackDuration = 0.1f;
    // private float cooldownClock;
    // private float cooldown = .2f;

    private bool canAttack = true;

    [SerializeField]
    SpriteRenderer attackHitboxDebug;
    [SerializeField]
    SpriteRenderer attackHitboxVDebug;
    [SerializeField]
    Collider2D attackHitbox;

    public void OnChargedThrust(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPressChargedThrust();
        }
    }

    private void OnPressChargedThrust()
    {
        if (canAttack)
        {
            canAttack = false;

            int flip = controller.FacingDirection;

            RaycastHit2D[] hits;

            
            hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, -transform.right * flip, 2);
            

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Breakable")
                {
                    //hit.collider.gameObject.GetComponent<EnemyBaseScript>().Hit(gameObject, gameObject.transform.position);
                    IHittable handler = hit.collider.GetComponent<IHittable>();
                    if (handler != null)
                        handler.OnHit(transform, 0);
                }
            }

            cooldownClock = cooldown;
        }
    }

    private void ChargedThrustUpdate()
    {
        if (cooldownClock > 0 && canAttack)
        {
            if (controller.Vertical >= .9) attackHitboxVDebug.enabled = true;
            else attackHitboxDebug.enabled = true;
            cooldownClock -= Time.deltaTime;
        }
        else
        {
            attackHitboxDebug.enabled = false;
            attackHitboxVDebug.enabled = false;
            canAttack = true;
        }
    }

    protected override IEnumerator VecShift()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        ChargedThrustUpdate();
    }

    
}

