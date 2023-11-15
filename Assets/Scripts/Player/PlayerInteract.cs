using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    CharacterController2D _controller;
    
    private float _interactTime;

    private bool _isInteracting = false;

    private float _interactCooldown = .3f;

    [SerializeField]
    private bool _canInteract = true;

   
    [SerializeField]
    Collider2D _interactHitbox;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
          
            OnPressInteract();
        }
    }

    private void OnPressInteract()
    {
        if (this._canInteract)
        {
            this._canInteract = false;

            int flip = 1;
            if(this._controller.IsFacingRight) flip = -1;

            RaycastHit2D[] hits;

            if (this._controller.Vertical >= .9f)
            {
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, transform.up, 2);
            }
            else
            {
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, -transform.right * flip, 2);
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    InteractableBaseScript interactable;
                    interactable = hit.collider.gameObject.GetComponent<InteractableBaseScript>();

                    if (interactable != null)
                    {
                      Debug.Log("Interacted");
                      interactable.OnInteract();
                    }
                }

               
            }

            StartCoroutine(Cooldown());

            this._interactTime = this._interactCooldown;
        }
    }

    private void InteractUpdate()
    {
        if (_isInteracting)
        {
            if (this._controller.Vertical >= .9) _interactHitbox.enabled = true;
            else  _interactHitbox.enabled = true;
            this._interactTime -= Time.deltaTime;
        }

        if (this._interactTime  <= 0)
        {
            this._interactHitbox.enabled = false;
            this._isInteracting = false;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(this._interactCooldown);
        this._canInteract = true;
    }


    IEnumerator VecShift() //temp
    {
        this._controller.ShiftTo3D();

        yield return new WaitForSeconds(1);

        this._controller.ShiftTo2D();
    }

    // Start is called before the first frame update
    private void Start()
    {
        this._controller = GetComponent<CharacterController2D>();
    }

   private void Update(){

        InteractUpdate();

   }

    
}

