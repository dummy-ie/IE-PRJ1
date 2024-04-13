using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GroundPound : AAbility
{
    Rigidbody2D rb;

    private bool isGroundPound = false;
    [SerializeField]
    private float boxCastDistance;
    [SerializeField]
    private Vector2 groundPoundHitBox;
    public bool IsGroundPound
    {
        get { return isGroundPound; }
    }

    [SerializeField]
    private float groundPoundFallMultiplier = 8.0f;

    private InputAction _groundPoundAction => InputManager.Instance.InputActions.Gameplay.GroundPound;

    private void OnPressGroundPound()
    {
        if (controller.Stats.HasPound)
        {
            if (controller.CanAttack && _groundPoundAction.WasPressedThisFrame())
            {
                // DISABLE INVISIBILITY
                controller.DeactivateInvisible();

                controller.CanAttack = false;

                rb.velocity = new Vector2(0.0f, 0.0f);
                controller.CurrentFallMultiplier = groundPoundFallMultiplier;
                isGroundPound = true;

                StartCoroutine(VecShift());
                //TriggerCooldown();
            }
        }
    }

    protected override IEnumerator VecShift() //temp
    {
        controller.ShiftTo3D();
        //controller.GetComponent<Rigidbody2D>().drag = 100f;

        //yield return new WaitForSeconds(.4f);

        controller.GetComponent<Rigidbody2D>().drag = 0;

        yield return new WaitForSeconds(.2f);

        
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        OnPressGroundPound();
        if (isGroundPound)
        {
            controller.CanMove = false;
            if (controller.IsGrounded())
            {
                RaycastHit2D[] hits;
                hits = Physics2D.BoxCastAll(transform.position, groundPoundHitBox, 0, -transform.up, boxCastDistance);
                foreach (RaycastHit2D hit in hits)
                {
                    //if (hit.collider.gameObject.layer == 6)
                    //    break;
                    IHittable handler = hit.collider.gameObject.GetComponentInParent<IHittable>();
                    Debug.Log(hit.transform.gameObject.name);
                    if (handler != null)
                    {
                        Debug.Log("hit");
                        handler.OnHit(transform, 3);
                    }
                }
                isGroundPound = false;
                controller.CanMove = true;
                controller.ResetFallMultiplier();
                controller.CanAttack = true;
                controller.ShiftTo2D();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - Vector3.up * boxCastDistance, groundPoundHitBox);
    }
}

