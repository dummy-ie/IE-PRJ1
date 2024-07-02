using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.XR;
using static CenturionBehaviour;


public class RailgunnerBehaviour : EnemyBase<RailgunnerBehaviour>, IEntityHittable
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] AttackData _hitscanAttack;
    [SerializeField] AttackData _burstAttack;

    [Header("Laser")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _timeToShoot = 1.5f;
    [SerializeField] private float _rotationSpeed = 10.0f;

    [Header("Follow Target")]
    [SerializeField] private float _followTargetSpeed;
    [SerializeField] private float _followWhileAttackSpeed;
    [SerializeField] private float _backwardSpeed;

    [Header("Patrol")]
    [SerializeField, Range(0, 100)] private float _patrolChance;
    [SerializeField, RangedValue(0, 30)] private RangeFloat _patrolTime;
    [SerializeField] private float _patrolMoveSpeed;

    private GameObject _playerTarget;

    private DeathState _deathState;
    private IdleState _idleState;
    private PatrolState _patrolState;
    private HurtState _hurtState;
    private FollowTargetState _followTargetState;
    private HitscanAttackState _hitscanAttackState;
    private BurstAttackState _burstAttackState;

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
        _hitscanAttackState = new HitscanAttackState(this);
        _burstAttackState = new BurstAttackState(this);

        InvokeRepeating("UpdatePath", 0f, PathUpdateSeconds);

        SwitchState(_idleState);
    }
    public void SetTarget(GameObject target)
    {
        this._playerTarget = target;
    }
    private bool ShouldFollowTarget() => _visionBehaviour.PlayerSeen && FollowEnabled;
    // TODO CHANGE EVERYTHING TO DISTANCE INSTEAD OF COLLIDER BEHAVIOURS AND HAVE A DISTANCE TO ATTACK SERIALIZED VARIABLE
    private bool ShouldFollowWhileAttack() => Vector2.Distance(transform.position, _playerTarget.transform.position) > 6;
    private bool TryEnterHitscanAttackState() => Vector2.Distance(transform.position, _playerTarget.transform.position) < 6;
    private bool TryEnterBurstAttackState() => Vector2.Distance(transform.position, _playerTarget.transform.position) < 2;
    private bool ShouldFlip() => _wallDetectBehaviour.WallDetected || (_cliffDetectBehaviour.CliffDetected && IsGrounded());
    private void UpdatePath()
    {
        if (ShouldFollowTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, _playerTarget.transform.position, OnPathComplete);
        }
    }
    public void OnHit(HitData hitData)
    {
        _currentHealth -= (int)hitData.damage;
        if (_currentHealth > 0)
        {
            _hurtState.lastHitData = hitData;
            SwitchState(_hurtState);
        }
        else SwitchState(_deathState);
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        FlipToGameObject(_playerTarget);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float force = _followTargetSpeed * _facingDirection;

        if (JumpEnabled && IsGrounded() && !isJumpOnCooldown)
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
        }

        rb.velocity = new Vector2(force, rb.velocity.y);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < NextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public abstract class StateBase : EntityState<RailgunnerBehaviour>
    {
        protected StateBase(RailgunnerBehaviour entity) : base(entity) { }
    }

    public class IdleState : StateBase
    {
        private float currentTick;
        public IdleState(RailgunnerBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            _entity.rb.velocity = Vector2.zero;
            currentTick = 0;
        }

        public override void Execute()
        {
            if (_entity.ShouldFollowTarget())
            {
                _entity.SwitchState(_entity._followTargetState);
                return;
            }

            currentTick += Time.deltaTime;
            if (currentTick >= 1)
            {

                if (UnityEngine.Random.Range(0.0f, 100.0f) <= _entity._patrolChance)
                {
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

        public PatrolState(RailgunnerBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            walkTime = _entity._patrolTime.RandomRange();
            _entity.RandomizeFlip();
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
            if (_entity.ShouldFlip())
            {
                _entity.Flip(_entity._facingDirection * -1);
            }

            _entity.rb.velocity = new Vector2(_entity._facingDirection * _entity._patrolMoveSpeed,
                _entity.rb.velocity.y);
        }
    }

    // TODO
    public class DeathState : StateBase
    {
        private float elapsedTime;
        public DeathState(RailgunnerBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            elapsedTime = 0;
            _entity.DropParticle(_entity._particleDropsOnDeath);
        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 2)
            {
                _entity.DropParticle(_entity._particleDropsOnDeath);
                Destroy(_entity.gameObject);
            }
        }
    }

    // TODO
    public class HurtState : StateBase
    {
        private float elapsedTime;
        public HitData lastHitData;
        public HurtState(RailgunnerBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            elapsedTime = 0;
            _entity.DropParticle(_entity._particleDropsOnHit);
            _entity.rb.velocity = Vector2.zero;
            _entity.rb.AddForce(lastHitData.force, ForceMode2D.Impulse);
        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 0.6f)
                _entity.SwitchState(_entity._idleState);
        }
    }

    public class FollowTargetState : StateBase
    {
        public FollowTargetState(RailgunnerBehaviour entity) : base(entity) { }

        public override void Execute()
        {
            if (_entity.TryEnterHitscanAttackState())
            {
                _entity.SwitchState(_entity._hitscanAttackState);
                return;
            }

            if (_entity.TryEnterBurstAttackState())
            {
                _entity.SwitchState(_entity._burstAttackState);
                return;
            }

            if (!_entity.ShouldFollowTarget())
            {
                _entity.SwitchState(_entity._patrolState);
                return;
            }

            _entity.PathFollow();
        }
    }

    public class HitscanAttackState : StateBase
    {
        private Transform _target;
        private Vector2 _direction;
        private Vector2 _raycastDirection;

        private LineRenderer _line;

        private float _trackingTicks = 0.0f;

        public HitscanAttackState(RailgunnerBehaviour entity) : base(entity) {}

        public override void Enter()
        {
            _entity.rb.velocity = Vector2.zero;
            _trackingTicks = 0.0f;
            _target = _entity._playerTarget.transform;
            _line = _entity._line;
            _line.widthMultiplier = 0.05f;
        }

        public override void Execute()
        {
            _trackingTicks += Time.deltaTime;

            _line.positionCount = 2;
            _direction = _target.position - _line.transform.position;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _line.transform.rotation = Quaternion.Slerp(_line.transform.rotation, rotation,
                _entity._rotationSpeed * Time.deltaTime);
            //_line.transform.rotation = rotation;

            _raycastDirection = _line.transform.right;
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);


            _line.SetPosition(0, _line.transform.position);
            _line.SetPosition(1, _raycastDirection * 10);

            _entity.FlipToGameObject(_entity._playerTarget);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                    _line.SetPosition(1, hit.point);
            }

            if (!_entity._rangeBehaviour.InRange)
            {
                _line.positionCount = 0;
                _entity.SwitchState(_entity._idleState);
            }

            _line.widthMultiplier = Mathf.Lerp(0.1f, 0.5f, _trackingTicks / _entity._timeToShoot);

            if (!_entity._visionBehaviour.PlayerSeen)
            {
                _line.widthMultiplier = 0.5f;
                _line.positionCount = 0;
                _entity.SwitchState(_entity._idleState);
            }

            if (_trackingTicks >= _entity._timeToShoot)
            {
                _line.widthMultiplier = 0.5f;
                PerformAttack(_entity._hitscanAttack);
                _line.positionCount = 0;
                _entity.SwitchState(_entity._idleState);
            }

            if (_entity.TryEnterBurstAttackState())
            {
                _entity.SwitchState(_entity._burstAttackState);
                return;
            }
        }

        public override void Exit()
        {
            _line.widthMultiplier = 0.5f;
            _line.positionCount = 0;
        }

        private void PerformAttack(AttackData attack)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    HitData hitData = new HitData(attack.damage, new Vector2(attack.knockbackForce.x * _entity._facingDirection, attack.knockbackForce.y));
                    hit.collider.GetComponent<CharacterController2D>().StartHit(hitData);
                }
            }
        }
    }
    public class BurstAttackState : StateBase
    {
        private Collider2D[] _hits = new Collider2D[8];
        private int _burstAttackCount;
        private float elapsedTime;

        public BurstAttackState(RailgunnerBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            elapsedTime = 0;
            _burstAttackCount = 0;
            _entity.FlipToGameObject(_entity._playerTarget);
            //TODO : PLAY ANIMATION STATE
        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 0.4f)
            { 
                PerformAttack(_entity._burstAttack);
                _burstAttackCount++;
                elapsedTime = 0;
            }

            if (_burstAttackCount > 5)
                _entity.SwitchState(_entity._idleState);

            _entity.rb.velocity = new Vector2(-_entity._facingDirection * _entity._backwardSpeed, _entity.rb.velocity.y);
        }
        private void PerformAttack(AttackData attack)
        {
            _entity.FlipToGameObject(_entity._playerTarget);

            float hitDistance = attack.moveOffset;// * _entity.m_RngStrength.RandomRange();
            RaycastHit2D hit = Physics2D.Raycast(_entity.rb.position, new Vector2(_entity._facingDirection, 0).normalized, hitDistance, _entity._groundLayer);

            CombatUtility.CastEntityBoxHit(_entity.rb.position + (attack.attackCollision.center * _entity.transform.localScale),
                attack.attackCollision.size, _hits, _entity._playerLayer, attack.damage,
                new Vector2(attack.knockbackForce.x * _entity._facingDirection, attack.knockbackForce.y));
        }
    }

    public void OnDrawGizmos()
    {
        DrawAttack(_burstAttack);
        void DrawAttack(AttackData attack)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(attack.attackCollision.x, attack.attackCollision.y),
                new Vector3(attack.attackCollision.width, attack.attackCollision.height));
        }
    }
}
