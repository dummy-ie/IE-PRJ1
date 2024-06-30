using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    CharacterController2D _controller;
    private Animator _animator;

    private bool isAttacking = false;
    private bool _doNextAttack = false;
    //private float attackDuration = 0.1f;
    private float attackTime;

    private int _attackStringNumber = 0;
    //[SerializeField]
    //private bool canAttack = true;

    [SerializeField]
    SpriteRenderer attackHitboxDebug;
    [SerializeField]
    SpriteRenderer attackHitboxVDebug;
    [SerializeField]
    Collider2D attackHitbox;

    private InputAction _attackAction => InputReader.Instance.InputActions.Gameplay.Attack;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackUpdate();
        OnPressAttack();
    }

    private void OnPressAttack()
    {
        if (!_doNextAttack && _attackAction.WasPressedThisFrame())
            _doNextAttack = true;
        if (!isAttacking && _doNextAttack && _attackAction.WasPressedThisFrame())
        {
            _doNextAttack = false;
            _controller.GetComponentInChildren<Animator>().Play($"Attack {_attackStringNumber + 1}");
            _attackStringNumber++;
            _attackStringNumber = _attackStringNumber % 3;
            // DISABLE INVISIBILITY
            _controller.DeactivateInvisible();

            AudioManager.Instance.PlaySFX(EClipIndex.NORMAL_ATTACK, transform.position);
            isAttacking = true;
            _controller.CanAttack = false;

            RaycastHit2D[] hits;

            if (_controller.Vertical >= .9f)
            {
                _animator.Play("animTrudeeUprightAttack");
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, transform.up, 2);
            }
            else if (_controller.Vertical <= -.9f && !_controller.IsGrounded())
            {
                _animator.Play("animTrudeeDownrightAttack");
                hits = Physics2D.BoxCastAll(transform.position, new Vector2(.5f, .5f), 0, -transform.up, 2);
            }
            else
            {
                Vector2 hitBoxSize = new Vector2(_controller.Data.FirstAttack.TriggerRect.width, _controller.Data.FirstAttack.TriggerRect.height);
                //float hitBoxDistance = Vector2.Distance(transform.position, new Vector2(_controller.Data.FirstAttack.TriggerRect.x, _controller.Data.FirstAttack.TriggerRect.y));
                hits = Physics2D.BoxCastAll(transform.position, hitBoxSize, 0, transform.right * _controller.FacingDirection, _controller.Data.FirstAttack.TriggerRect.x);
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.layer == 6)
                    break;
                IEntityHittable handler = hit.collider.gameObject.GetComponent<IEntityHittable>();
                if (handler != null)
                {
                    HitData hitData = new HitData(_controller.Data.FirstAttack.Damage,
                        new Vector2(_controller.Data.FirstAttack.HorizontalMoveOffset, 0));
                    handler.OnHit(hitData);
                }
            }

            TriggerCooldown();
            attackTime = _controller.Data.FirstAttack.Cooldown;
        }
    }

    private void AttackUpdate()
    {
        if (isAttacking)
        {
            if (_controller.Vertical >= .9) attackHitboxVDebug.enabled = true;
            else attackHitboxDebug.enabled = true;
            attackTime -= Time.deltaTime;
        }

        if (attackTime <= 0)
        {
            attackHitboxDebug.enabled = false;
            attackHitboxVDebug.enabled = false;
            isAttacking = false;
        }

        if (!isAttacking && !_doNextAttack)
            _attackStringNumber = 0;
    }

    void TriggerCooldown()
    {
        _controller.StartCooldown(_controller.Data.FirstAttack.Cooldown);
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
        _controller.ShiftTo3D();

        yield return new WaitForSeconds(1);

        _controller.ShiftTo2D();
    }

    

}
