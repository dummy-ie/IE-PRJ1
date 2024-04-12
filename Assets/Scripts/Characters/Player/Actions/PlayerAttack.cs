using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    CharacterController2D _controller;
    private Animator _animator;

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

    private InputAction _attackAction => InputManager.Instance.InputActions.Player.Attack;

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
        if (_controller.Data.CanAttack && _attackAction.IsPressed())
        {
            // DISABLE INVISIBILITY
            _controller.DeactivateInvisible();

            AudioManager.Instance.PlaySFX(EClipIndex.NORMAL_ATTACK);
            isAttacking = true;
            _controller.Data.CanAttack = false;

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
                hits = Physics2D.BoxCastAll(transform.position, _controller.Data.FirstAttack.HitboxSize, 0, transform.right * _controller.FacingDirection, _controller.Data.FirstAttack.HitboxCastDistance);
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.layer == 6)
                    break;
                IHittable handler = hit.collider.gameObject.GetComponent<IHittable>();
                if (handler != null)
                {
                    handler.OnHit(transform, _controller.Data.FirstAttack.Damage);
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
