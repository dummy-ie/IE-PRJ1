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

    private InputAction _chargedThrustAction => InputManager.Instance.InputActions.Gameplay.ChargedThrust;

    private void OnPressChargedThrust()
    {
        if (controller.Stats.HasThrust && _chargedThrustAction.WasPressedThisFrame())
        {
            if (controller.CanAttack && controller.Stats.Manite.Current >= maniteCost)
            {
                canAttack = false;

                int flip = controller.FacingDirection;

                RaycastHit2D[] hits;


                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, -transform.right * flip, 2);


                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Breakable"))
                    {
                        //hit.collider.gameObject.GetComponent<EnemyBaseScript>().Hit(gameObject, gameObject.transform.position);
                        if (hit.collider.TryGetComponent<IHittable>(out var handler))
                        {
                            handler.OnHit(transform, 0);
                            Debug.Log("hit obj: " + hit.collider.gameObject.name);
                        }
                    }
                }
                StartCoroutine(VecShift());
                controller.Stats.Manite.Current -= maniteCost;
                cooldownClock = cooldown;
            }
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
        controller.ShiftTo3D();
        controller.GetComponent<Rigidbody2D>().drag = 100f;

        yield return new WaitForSeconds(.4f);

        controller.GetComponent<Rigidbody2D>().drag = 0;

        yield return new WaitForSeconds(.2f);

        controller.ShiftTo2D();
    }

    // Update is called once per frame
    void Update()
    {
        OnPressChargedThrust();
        ChargedThrustUpdate();
    }


}

