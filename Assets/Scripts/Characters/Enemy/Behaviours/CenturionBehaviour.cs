using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Rigidbody2D))]
public class CenturionBehaviour : EnemyBase<CenturionBehaviour>
{
    [Header("Attacks")]
    [SerializeField] private AttackData _initialSlash;
    
    
    [SerializeField] private AttackData _smashSlash;
    

    [SerializeField] private AttackData _aoeSlash;
    [SerializeField] private GameObject _projectileSlash;
    [SerializeField] private GameObject _smashSpikes;
    [SerializeField] private AttackData _backhandSwing;
    [SerializeField] private AttackData _forehandSwing;
    [SerializeField] private AttackData _shockwave;
    [SerializeField] private AttackData _jumpSmash;

    [SerializeField] private float _initialSlashTime = 2.0f;
    [SerializeField] private float _smashSlashTime = 2.0f;
    [SerializeField] private float _firstSpikeTime = 0.5f;
    [SerializeField] private float _secondSpikeTime = 0.5f;
    [SerializeField] private float _thirdSpikeTime = 0.5f;
    [SerializeField] private float _backhandSwingTime = 2.0f;
    [SerializeField] private float _forehandSwingTime = 2.0f;
    [SerializeField] private float _jumpSlashTime = 3.0f;
    [SerializeField] private float _firstAttackPatternTime = 7.0f;
    [SerializeField] private float _idleTime = 5.0f;

    private PreBattleState _preBattleState;
    private IdleState _idleState;
    private FirstAttackState _firstAttackState;
    private SecondAttackState _secondAttackState;
    private FirstAmuletState _firstAmuletState;
    private SecondAmuletState _secondAmuletState;
    private DeathState _deathState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _preBattleState = new PreBattleState(this);
        _idleState = new IdleState(this);
        _firstAttackState = new FirstAttackState(this);
        _secondAttackState = new SecondAttackState(this);
        _firstAmuletState = new FirstAmuletState(this);
        _secondAmuletState = new SecondAmuletState(this);
        _deathState = new DeathState(this);

        FlipTo();
        SwitchState(_preBattleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (_currentHealth <= 0)
            SwitchState(_deathState);
    }
    public abstract class StateBase : EntityState<CenturionBehaviour>
    {
        public StateBase(CenturionBehaviour entity) : base(entity) { }
    }

    public class PreBattleState : StateBase
    {
        public PreBattleState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
        }

        public override void Execute()
        {
            // TO DO : ANIMATION
            _entity.SwitchState(_entity._idleState);
        }
    }

    public class IdleState : StateBase
    {
        private int _patternCount;
        private float _ticks;

        public IdleState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _ticks = 0.0f;
            _patternCount = Random.Range(0, 4);
        }

        public override void Execute()
        {
            // TO DO : DO SOME IDLE SHIT
            _ticks += Time.deltaTime;

            if (_ticks >= _entity._idleTime)
            {
                Debug.Log("Pattern Count : " + _patternCount);
                switch (_patternCount)
                {
                    case 0:
                        _entity.SwitchState(_entity._firstAttackState);
                        break;
                    case 1:
                        _entity.SwitchState(_entity._firstAttackState);
                        //_entity.SwitchState(_entity._secondAttackState);
                        break;
                    case 2:
                        _entity.SwitchState(_entity._firstAttackState);
                        //_entity.SwitchState(_entity._firstAmuletState);
                        break;
                    case 3:
                        _entity.SwitchState(_entity._firstAttackState);
                        //_entity.SwitchState(_entity._secondAmuletState);
                        break;
                }
                //_entity.SwitchState(_entity._idleState);
            }
        }
    }

    public class FirstAttackState : StateBase
    {
        private float _elapsedTime;

        private bool _performedFirstAttack;
        private bool _performedSecondAttack;
        private bool _performedFirstSpike;
        private bool _performedSecondSpike;
        private bool _performedThirdSpike;

        private float _spikeOffset = 3.0f;
        public FirstAttackState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Debug.Log("hello");
            _entity.rb.velocity = Vector2.zero;
            _elapsedTime = 0.0f;
            _performedFirstAttack = false;
            _performedSecondAttack = false;
            _performedFirstSpike = false;
            _performedSecondSpike = false;
            _performedThirdSpike = false;
        }

        public override void Execute()
        {
            _elapsedTime += Time.deltaTime;

            if (!_performedFirstAttack && _elapsedTime >= _entity._initialSlashTime)
            {
                Debug.Log("Initial Slash");
                PerformAttack(_entity._initialSlash);
                _performedFirstAttack = true;
            }

            if (!_performedSecondAttack && _elapsedTime >= _entity._smashSlashTime + _entity._initialSlashTime)
            {
                int facingRight = 1;
                if (!_entity._isFacingRight)
                    facingRight = -1;
                PerformAttack(_entity._smashSlash);

                GameObject projectile = Instantiate(
                _entity._projectileSlash,
                new Vector3(_entity.transform.position.x + 3.0f * facingRight, _entity.transform.position.y, _entity.transform.position.z),
                Quaternion.identity);

                var temp = projectile.GetComponent<HorizontalProjectile>();
                temp.SourcePlayer = _entity.gameObject;
                temp.Damage = 1;

                Vector3 projectileScale = projectile.transform.localScale;
                projectileScale.y *= -facingRight;
                projectile.transform.localScale = projectileScale;

                // rotate dat bitch
                projectile.transform.Rotate(new Vector3(0f, 0f, -90f));

                _performedSecondAttack = true;
            }

            if (!_performedFirstSpike && _elapsedTime >= _entity._smashSlashTime + _entity._initialSlashTime + _entity._firstSpikeTime)
            {
                PerformAttack(_entity._aoeSlash);
                
                _performedFirstSpike = true;
            }

            if (_elapsedTime >= _entity._firstAttackPatternTime)
            {
                _entity.SwitchState(_entity._idleState);
            }
        }

        private void PerformAttack(AttackData attack)
        {
            int facingRight = 1;
            if (!_entity._isFacingRight)
                facingRight = -1;
            RaycastHit2D[] hits;
            Vector3 origin = _entity.transform.position + new Vector3(attack.attackCollision.x * facingRight,
                attack.attackCollision.y);
            hits = Physics2D.BoxCastAll(origin,
                new Vector2(attack.attackCollision.width, attack.attackCollision.height), 0,
                Vector2.zero, 0);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    HitData hitData = new HitData(attack.damage, attack.moveOffset * facingRight);
                    hit.collider.GetComponent<CharacterController2D>()
                        .StartHit(hitData);
                }
            }
        }
    }

    public class SecondAttackState : StateBase
    {
        public SecondAttackState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
        }

        public override void Execute()
        {
            _entity.SwitchState(_entity._idleState);
        }
    }

    public class FirstAmuletState : StateBase
    {
        public FirstAmuletState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
        }

        public override void Execute()
        {
            _entity.SwitchState(_entity._idleState);
        }
    }

    public class SecondAmuletState : StateBase
    {
        public SecondAmuletState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
        }

        public override void Execute()
        {
            _entity.SwitchState(_entity._idleState);
        }
    }

    public class DeathState : StateBase
    {
        public DeathState(CenturionBehaviour entity) : base(entity) { }
        public override void Enter()
        {
        }

        public override void Execute()
        {

        }
    }

    private void OnDrawGizmos()
    {
        DrawAttack(_initialSlash);
        DrawAttack(_smashSlash);
        DrawAttack(_aoeSlash);
        DrawAttack(_backhandSwing);
        DrawAttack(_forehandSwing);
        DrawAttack(_shockwave);
        DrawAttack(_jumpSmash);
        void DrawAttack(AttackData attack)
        {
            if (attack.drawGizmos)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(attack.attackCollision.x, attack.attackCollision.y),
                    new Vector3(attack.attackCollision.width, attack.attackCollision.height));
            }
        }
    }
}
