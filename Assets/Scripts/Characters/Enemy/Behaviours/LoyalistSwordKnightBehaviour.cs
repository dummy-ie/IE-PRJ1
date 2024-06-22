using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.XR;
using static UnityEngine.EventSystems.EventTrigger;


public class LoyalistSwordKnightBehaviour : EnemyBase<LoyalistSwordKnightBehaviour>
{
    [Header("Attack")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _attackDuration;
    [SerializeField] AttackData _firstSlash;
    [SerializeField] private float _firstSlashTime;
    [SerializeField] AttackData _secondSlash;
    [SerializeField] private float _secondSlashTime;
    [SerializeField] AttackData _thirdSlash;
    [SerializeField] private float _thirdSlashTime;
    [SerializeField] private float _attackCooldown;

    [Header("Follow Target")]
    [SerializeField] private float _followTargetSpeed;

    [Header("Patrol")]
    [SerializeField, Range(0, 100)] private float _patrolChance;
    [SerializeField, Range(0, 30)] private float _minPatrolTime;
    [SerializeField, Range(0, 30)] private float _maxPatrolTime;
    [SerializeField] private float _patrolMoveSpeed;
    [SerializeField] private GameObject _patrolTarget;
    [SerializeField] private GameObject _patrolTarget2;

    private GameObject _playerTarget;

    private DeathState _deathState;
    private IdleState _idleState;
    private PatrolState _patrolState;
    private HurtState _hurtState;
    private FollowTargetState _followTargetState;
    private AttackState _attackState;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        IsJumping = false;
        IsInAir = false;
        isJumpOnCooldown = false;

        _playerTarget = GameObject.FindGameObjectWithTag("Player");

        _deathState = new DeathState(this);
        _idleState = new IdleState(this);
        _patrolState = new PatrolState(this);
        _hurtState = new HurtState(this);
        _followTargetState = new FollowTargetState(this);
        _attackState = new AttackState(this);

        InvokeRepeating("UpdatePath", 0f, PathUpdateSeconds);

        SwitchState(_idleState);
    }

    public void SetTarget(GameObject target)
    {
        this._playerTarget = target;
    }
    private bool ShouldFollowTarget() => _visionBehaviour.PlayerSeen && FollowEnabled;
    private bool TryEnterAttackState() => _rangeBehaviour.InRange;
    private void UpdatePath()
    {
        if (ShouldFollowTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, _playerTarget.transform.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;
        
        FlipTo(_playerTarget.transform.position.x);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float force = _followTargetSpeed * _facingDirection;

        if (JumpEnabled && IsGrounded() && !IsInAir && !isJumpOnCooldown)
        {
            if (direction.y > JumpNodeHeightRequirement)
            {
                IsJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                StartCoroutine(JumpCooldown());
            }
        }

        if (IsGrounded())
        {
            IsJumping = false;
            IsInAir = false;
        }
        else
        {
            IsInAir = true;
        }

        rb.velocity = new Vector2(force, rb.velocity.y);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < NextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public abstract class StateBase : EntityState<LoyalistSwordKnightBehaviour>
    {
        protected StateBase(LoyalistSwordKnightBehaviour entity) : base(entity) { }
    }

    public class IdleState : StateBase
    {
        private float currentTick;
        public IdleState(LoyalistSwordKnightBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            Debug.Log("ENTER IWDILEO");
            _entity.rb.velocity = Vector2.zero;
            currentTick = 0;
        }

        public override void Execute()
        {
            if (_entity.ShouldFollowTarget()) {
                
                _entity.SwitchState(_entity._followTargetState);
                return;
            }

            currentTick += Time.deltaTime;
            if (currentTick >= 1)
            {
                
                if (UnityEngine.Random.Range(0.0f, 100.0f) <= _entity._patrolChance)
                {
                    Debug.Log("??");
                    _entity.SwitchState(_entity._patrolState);
                }
                    
                currentTick -= 1;
            }
        }
    }
    public class PatrolState : StateBase
    {
        //bool goToPointB = false;

        private float elapsedTime;
        private float walkTime;

        public PatrolState(LoyalistSwordKnightBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            Debug.Log("LOYALIST ENTER PATROL");
            walkTime = UnityEngine.Random.Range(_entity._minPatrolTime, _entity._maxPatrolTime);
            _entity.Flip(System.Convert.ToBoolean(Random.Range(0, 2)) ? 1 : -1);
            elapsedTime = 0;
        }
        public override void Execute()
        {
            /*if (_entity._visionBehaviour.PlayerSeen == false)
            {
                if (goToPointB == false)
                {
                    Vector2 pos = new(_entity.transform.position.x - (_entity.GetDirection(_entity._patrolTarget)/* * _entity.Speed), 0.0f);
                    _entity.transform.position = Vector2.MoveTowards(_entity.rb.transform.position, pos, Time.deltaTime * _entity.Speed);

                    if (fTicks >= fTimeInterval)
                    {
                        fTicks = 0.0f;
                        goToPointB = true;
                    }

                    fTicks += 0.05f;

                    _entity.SwitchState(_entity._patrolState);
                }
                else if (goToPointB == true)
                {
                    Vector2 pos2 = new(_entity.transform.position.x + (_entity.GetDirection(_entity._patrolTarget2)/* * _entity.Speed), 34.0f);
                    _entity.transform.position = Vector2.MoveTowards(_entity.rb.transform.position, pos2, Time.deltaTime * _entity.Speed);

                    if (fTicks >= fTimeInterval)
                    {
                        fTicks = 0.0f;
                        goToPointB = false;
                    }

                    fTicks += 0.05f;

                    _entity.SwitchState(_entity._patrolState);
                }
            }*/

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= walkTime)
            {
                _entity.SwitchState(_entity._idleState);
                return;
            }

            if (_entity.ShouldFollowTarget())
            {
                _entity.SwitchState(_entity._followTargetState);
                return;
            }

            // TODO: Add a flip when it hits a wall or a cliff
            Debug.Log("WAWAW");
            _entity.rb.velocity = new Vector2(_entity._facingDirection * _entity._patrolMoveSpeed,
                _entity.rb.velocity.y);
        }
    }
    public class DeathState : StateBase
    {
        public DeathState(LoyalistSwordKnightBehaviour entity) : base(entity) { }
    }

    public class HurtState : StateBase
    {
        public HurtState(LoyalistSwordKnightBehaviour entity) : base(entity) { }
    }

    public class FollowTargetState : StateBase
    {
        bool goToPointB = false;
        float fTimeInterval = 150.0f;

        float fTicks = 0.0f;

        public FollowTargetState(LoyalistSwordKnightBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            Debug.LogWarning("FOLLOWING TARGET");
        }

        public override void Execute()
        {
            if (_entity.TryEnterAttackState())
            {
                _entity.SwitchState(_entity._attackState);
                return;
            }
            
            if (!_entity.ShouldFollowTarget())
            {
                Debug.LogWarning("stio folginwfing");
                _entity.SwitchState(_entity._patrolState);
                return;
            }

            _entity.PathFollow();
        }
    }

    public class AttackState : StateBase
    {
        private Collider2D[] _hits = new Collider2D[8];
        private float elapsedTime;
        private bool performedFirstSlash;
        private bool performedSecondSlash;
        private bool performedThirdSlash;
        public AttackState(LoyalistSwordKnightBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _entity.rb.velocity = Vector2.zero;
            performedFirstSlash = false;
            performedSecondSlash = false;
            performedThirdSlash = false;
            elapsedTime = 0;
            _entity.FlipTo(_entity._playerTarget.transform.position.x); // TODO: Make this into a function so I don't have to type it all
            //entity.PlayAnimation("Attack");
        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;

            if (!performedFirstSlash && elapsedTime >= _entity._firstSlashTime)
            {
                PerformAttack(_entity._firstSlash);
                performedFirstSlash = true;
            }

            if (!performedSecondSlash && elapsedTime >= _entity._secondSlashTime)
            {
                PerformAttack(_entity._secondSlash);
                performedSecondSlash = true;
            }

            if (!performedThirdSlash && elapsedTime >= _entity._thirdSlashTime)
            {
                PerformAttack(_entity._thirdSlash);
                performedThirdSlash = true;
            }

            if (elapsedTime >= _entity._attackDuration)
            {
                _entity.SwitchState(_entity._idleState);
            }
        }

        private void PerformAttack(AttackData attack)
        {
            _entity.FlipTo(_entity._playerTarget.transform.position.x);

            float hitDistance = attack.moveOffset;// * _entity.m_RngStrength.RandomRange();
            RaycastHit2D hit = Physics2D.Raycast(_entity.rb.position, new Vector2(_entity._facingDirection, 0).normalized, hitDistance, _entity._groundLayer);
            if (hit)
                _entity.rb.MovePosition(new Vector2(hit.point.x - (0.5f * _entity._facingDirection * 1.5f/*_entity.bounds.size.x*/), hit.point.y));
            else
                _entity.rb.MovePosition(new Vector2(_entity.rb.position.x + (hitDistance * _entity._facingDirection), _entity.rb.position.y));

            CastEntityBoxHit(_entity.rb.position + (attack.attackCollision.center * _entity.transform.localScale),
                attack.attackCollision.size, _hits, _entity._playerLayer, attack.damage,
                new Vector2(attack.knockbackForce.x * _entity._facingDirection, attack.knockbackForce.y));
        }

        // TODO : PUT THIS INTO A UTILITY CLASS
        public void CastEntityBoxHit(Vector2 position, Vector2 size, Collider2D[] hits, LayerMask targetLayer, float damage, Vector2 knockbackForce)
        {
            int hitsCount = Physics2D.OverlapBoxNonAlloc(position, size, 0, hits, targetLayer);
            if (hitsCount == 0)
                return;

            HitData hitData = new HitData(damage, knockbackForce);

            for (int i = 0; i < hitsCount; i++)
            {
                Collider2D hit = hits[i];
                if (hit.gameObject.CompareTag("Player"))
                {
                    // TODO : MAKE THE PLAYER BE IHITTABLE TOO
                    hit.GetComponent<CharacterController2D>().StartHit(hitData);
                    //if (hit.TryGetComponent<IHittable>(out IHittable hittableTarget))
                    //hittableTarget.OnHit(hitData);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        DrawAttack(_firstSlash);
        DrawAttack(_secondSlash);
        DrawAttack(_thirdSlash);
        void DrawAttack(AttackData attack)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(attack.attackCollision.x, attack.attackCollision.y),
                new Vector3(attack.attackCollision.width, attack.attackCollision.height));
        }
    }
}
