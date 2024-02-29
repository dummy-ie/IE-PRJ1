using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class GroundPound : AAbility
{

    CharacterController2D controller;
    Rigidbody2D rb;

    private bool isGroundPound = false;

    public bool IsGroundPound
    {
        get { return isGroundPound; }
    }

    [SerializeField]
    private float groundPoundFallMultiplier = 8.0f;

    public void OnGroundPound(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPressGroundPound();
        }
    }

    private void OnPressGroundPound()
    {
                Debug.Log("ground pound");
        if (controller.Stats.HasPound)
        {
            if (controller.Data.CanAttack)
            {
                controller.Data.CanAttack = false;

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
        if (isGroundPound)
        {
            if (controller.IsGrounded())
            {
                RaycastHit2D[] hits;
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(2f, 2f), 0, -transform.up, 0);
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
                controller.ResetFallMultiplier();
                controller.Data.CanAttack = true;
                controller.ShiftTo2D();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - Vector3.up, new Vector2(2f, 2f));
    }
}

