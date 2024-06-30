using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroneBehaviour : EnemyBase< AttackDroneBehaviour>, IEntityHittable
{

    [Header("Attack")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _attackCooldown;
    //boolet

    [Header("Follow Target")]
    [SerializeField] private float _followTargetSpeed;

    [Header("Patrol")]
    [SerializeField, Range(0, 100)] private float _patrolChance;
    [SerializeField] private RangeFloat _patrolTime;
    [SerializeField] private float _patrolMoveSpeed;

    private float _attackCooldownTicks = 0;
    private bool _attackOnCooldown;

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

    void Update()
    {
        if (_attackOnCooldown)
        {
            _attackCooldownTicks += Time.deltaTime;
            if (_attackCooldownTicks >= _attackCooldown)
            {
                _attackCooldownTicks = 0;
                _attackOnCooldown = false;
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        this._playerTarget = target;
    }
    private bool ShouldFollowTarget() => _visionBehaviour.PlayerSeen && FollowEnabled;
    private bool TryEnterAttackState() => _rangeBehaviour.InRange && !_attackOnCooldown;
    private bool ShouldFlip() => _wallDetectBehaviour.WallDetected || (_cliffDetectBehaviour.CliffDetected && IsGrounded());
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

        FlipToGameObject(_playerTarget);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float force = _followTargetSpeed * _facingDirection;

        rb.velocity = new Vector2(force, force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < NextWaypointDistance)
        {
            currentWaypoint++;
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

    public abstract class StateBase : EntityState< AttackDroneBehaviour>
    {
        protected StateBase( AttackDroneBehaviour entity) : base(entity) { }
    }

    public class IdleState : StateBase
    {
        private float currentTick;
        public IdleState( AttackDroneBehaviour entity) : base(entity) { }

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
                    Debug.Log("??");
                    _entity.SwitchState(_entity._patrolState);
                }

                currentTick -= 1;
            }
        }
    }
    public class PatrolState : StateBase
    {

        private float elapsedTime;
        private float walkTime;

        public PatrolState( AttackDroneBehaviour entity) : base(entity) { }

        public override void Enter()
        {

            walkTime = _entity._patrolTime.RandomRange();
            _entity.RandomizeFlip();
            elapsedTime = 0;
        }
        public override void Execute()
        {


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
            Debug.LogWarning(_entity.IsGrounded());
            if (_entity.ShouldFlip())
            {

                _entity.Flip(_entity._facingDirection * -1);
            }

            _entity.rb.velocity = new Vector2(_entity._facingDirection * _entity._patrolMoveSpeed,
                _entity._facingDirection * _entity._patrolMoveSpeed);
        }
    }
    public class DeathState : StateBase
    {
        private float elapsedTime;
        public DeathState( AttackDroneBehaviour entity) : base(entity) { }

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
                Destroy(_entity.gameObject);
            }
        }
    }

    public class HurtState : StateBase
    {
        private float elapsedTime;
        public HitData lastHitData;
        public HurtState( AttackDroneBehaviour entity) : base(entity) { }
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
        public FollowTargetState( AttackDroneBehaviour entity) : base(entity) { }

        public override void Execute()
        {
            if (_entity.TryEnterAttackState())
            {
                _entity.SwitchState(_entity._attackState);
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

    public class AttackState : StateBase
    {
        private Collider2D[] _hits = new Collider2D[8];
        private float elapsedTime;
        public AttackState( AttackDroneBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _entity.rb.velocity = Vector2.zero;
            elapsedTime = 0;
            _entity.FlipToGameObject(_entity._playerTarget);
            //TODO : PLAY ANIMATION STATE
        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= _entity._attackDelay)
            {
                //PerformAttack();
                Debug.Log("Attack Drone shoots...");
                _entity.SwitchState(_entity._idleState);
            }
        }

        public override void Exit()
        {
            _entity._attackCooldownTicks = 0.0f;
            _entity._attackOnCooldown = true;
        }

        private void PerformAttack(AttackData attack)
        {
            _entity.FlipToGameObject(_entity._playerTarget);

            float hitDistance = attack.moveOffset;// * _entity.m_RngStrength.RandomRange();
            RaycastHit2D hit = Physics2D.Raycast(_entity.rb.position, new Vector2(_entity._facingDirection, 0).normalized, hitDistance, _entity._groundLayer);

            // TODO: POSSIBLY CHANGE THIS SO IT'S A FADE MOVE INSTEAD OF INSTANTLY MOVING THE MODEL
            if (hit)
                _entity.rb.MovePosition(new Vector2(hit.point.x - (0.5f * _entity._facingDirection * 1.5f/*_entity.bounds.size.x*/), hit.point.y));
            else
                _entity.rb.MovePosition(new Vector2(_entity.rb.position.x + (hitDistance * _entity._facingDirection), _entity.rb.position.y));

            CombatUtility.CastEntityBoxHit(_entity.rb.position + (attack.attackCollision.center * _entity.transform.localScale),
                attack.attackCollision.size, _hits, _entity._playerLayer, attack.damage,
                new Vector2(attack.knockbackForce.x * _entity._facingDirection, attack.knockbackForce.y));
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - Vector3.up * _boxCastDistance, _boxSize);
        void DrawAttack(AttackData attack)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(attack.attackCollision.x, attack.attackCollision.y),
                new Vector3(attack.attackCollision.width, attack.attackCollision.height));
        }
    }
}
