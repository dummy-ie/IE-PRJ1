
using UnityEngine;

using static PlayerData;

[RequireComponent(typeof(Rigidbody2D))]
public class ExoArtilleryBehaviour : EnemyBase<ExoArtilleryBehaviour>
{

    [SerializeField] AttackData _stompAttack;
    [SerializeField] AttackData _sniperAttack;

    [SerializeField] private Transform _center;

    [Header("Robot")]
    [SerializeField] private GameObject _rightArm;
    [SerializeField] private GameObject _leftArm;

    [Header("Laser")]
    [SerializeField] private Transform _target;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private LineRenderer _line2;
    [SerializeField] private float _timeToShoot = 1.5f;
    [SerializeField] private float _rotationSpeed = 10.0f;

    private int _phase1Count = 0;
    private bool _firstAttack = true;

    private PreBattleState _preBattleState;
    private Phase1State _phase1State;
    private Phase1LeapState _phase1LeapState;
    private DamagePhaseState _damagePhaseState;
    private Phase2State _phase2State;
    private DeathState _deathState;
    virtual public void OnHit(Transform source, int damage)
    {
    }

    protected virtual void Start()
    {
        _preBattleState = new PreBattleState(this);
        _phase1State = new Phase1State(this);
        _phase1LeapState = new Phase1LeapState(this);
        _damagePhaseState = new DamagePhaseState(this);
        _deathState = new DeathState(this);
        FlipTo();
        SwitchState(_preBattleState);
    }

    protected override void Update()
    {
        base.Update();
        if (_currentHealth <= 0)
            SwitchState(_deathState);
    }

    public abstract class StateBase : EntityState<ExoArtilleryBehaviour>
    {
        public StateBase(ExoArtilleryBehaviour entity) : base(entity) {}
    }

    public class PreBattleState : StateBase
    {
        public PreBattleState(ExoArtilleryBehaviour entity) : base(entity) {}
        public override void Enter()
        {
        }

        public override void Execute()
        {
            if (_entity.IsGrounded())
            {
                _entity.SwitchState(_entity._phase1State);
            }
        }
    }

    public class Phase1State : StateBase
    {
        private Vector3 _oldPos;
        private Vector3 _newPos;

        private bool _stomping = false;
        private bool _tracking = true;

        private float _stompTicks = 0.0f;
        private float _stompTimer = 0.0f;
        private float _stompTime = 0.5f;
        private float _mortarTicks = 0.0f;
        private float _elapsedTime = 0.0f;

        private float _trackingTicks = 0.0f;

        private Transform _target;
        private Vector2 _direction;
        private Vector2 _direction2;
        private Vector2 _raycastDirection;

        private LineRenderer _line;
        private LineRenderer _line2;

        public Phase1State(ExoArtilleryBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            _oldPos = _entity.transform.position;
            _stompTicks = 0.0f;
            _mortarTicks = 0.0f;
            _elapsedTime = 0.0f;

            _target = _entity._target;
            _line = _entity._line;
            _line2 = _entity._line2;
        }

        public override void Execute()
        {
            
            _stompTicks += Time.deltaTime;
            _mortarTicks += Time.deltaTime;
            _elapsedTime += Time.deltaTime;


            if (_stompTicks >= 1.0f)
            {
                int facingRight = 1;
                if (!_entity._isFacingRight)
                    facingRight = -1;
                _stompTicks = 0.0f;
                _stomping = true;
                _oldPos = _entity.transform.position;
                _newPos = _entity.transform.position += new Vector3(2.0f * facingRight, 0); ;
            }

            if (_stomping)
            {
                _stompTimer += Time.deltaTime;
                _entity.transform.position = Vector3.Slerp(_oldPos, _newPos, _stompTimer / _stompTime);

                if (_stompTimer >= _stompTime)
                {
                    _stompTimer = 0.0f;
                    _stomping = false;
                    PerformAttack(_entity._stompAttack);
                }
            }

            if (_mortarTicks >= 1.0f)
            {
                PerformMortar();
            }

            _tracking = _entity._rangeBehaviour.InRange;

            if (_tracking && !_entity._firstAttack)
            {
                _trackingTicks += Time.deltaTime;

                _line.positionCount = 2;
                _direction = _target.position - _line.transform.position;
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _line.transform.rotation = Quaternion.Slerp(_line.transform.rotation, rotation,
                    _entity._rotationSpeed * Time.deltaTime);
                Quaternion armRotation = Quaternion.AngleAxis(angle, Vector3.right);
                _entity._rightArm.transform.localRotation = Quaternion.Slerp(_entity._rightArm.transform.localRotation, armRotation,
                    _entity._rotationSpeed * Time.deltaTime);

                _line2.positionCount = 2;
                _direction2 = _target.position - _line2.transform.position;
                angle = Mathf.Atan2(_direction2.y, _direction2.x) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle, Vector3.right);
                _line2.transform.rotation = Quaternion.Slerp(_line2.transform.rotation, rotation,
                    _entity._rotationSpeed * Time.deltaTime);
                armRotation = Quaternion.AngleAxis(angle, Vector3.right);
                _entity._leftArm.transform.localRotation = Quaternion.Slerp(_entity._leftArm.transform.localRotation, armRotation,
                    _entity._rotationSpeed * Time.deltaTime);

                _raycastDirection = _line.transform.right;
                RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);

                _line.SetPosition(0, _line.transform.position);
                _line.SetPosition(1, _raycastDirection * 10);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                        _line.SetPosition(1, hit.point);
                }

                _raycastDirection = _line2.transform.right;
                hits = Physics2D.RaycastAll(_line2.transform.position, _raycastDirection);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                        _line2.SetPosition(1, hit.point);
                }
                _line2.SetPosition(0, _line2.transform.position);
                _line2.SetPosition(1, _raycastDirection * 10);

                //_entity._leftArm.transform.LookAt(_line2.GetPosition(1));
                //_entity._rightArm.transform.LookAt(_line.GetPosition(1));

                if (!_entity._rangeBehaviour.InRange)
                {
                    _line.positionCount = 0;
                    _line2.positionCount = 0;
                }
                _line.widthMultiplier = Mathf.Lerp(0.1f,0.5f, _trackingTicks / _entity._timeToShoot);
                _line2.widthMultiplier = Mathf.Lerp(0.1f,0.5f, _trackingTicks / _entity._timeToShoot);
                if (_trackingTicks >= _entity._timeToShoot)
                {
                    PerformSniper(_entity._sniperAttack);
                    _trackingTicks = 0.0f;
                }
            }
            else
            {
                _line.positionCount = 0;
                _line2.positionCount = 0;
                _trackingTicks = 0.0f;
            }


            if (_elapsedTime >= 13.0f)
            {
                _entity.SwitchState(_entity._phase1LeapState);
            }
        }
        public override void Exit()
        {
            _line.positionCount = 0;
            _line2.positionCount = 0;
            _entity._firstAttack = false;
            //SwitchState(null);
        }

        private void PerformSniper(AttackData attack)
        {
            Debug.Log("Railgunner performed attack");
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.GetComponent<CharacterController2D>().StartHit(_entity.gameObject, attack.damage);
                }
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
                    hit.collider.GetComponent<CharacterController2D>()
                        .StartHit(_entity.gameObject, attack.damage);
                }
            }
        }
        private void PerformMortar()
        {

        }
    }

    public class Phase1LeapState : StateBase
    {
        private Vector3 _oldPos;
        private Vector3 _nextPos;
        private float _leapTime = 2.5f;
        private float _ticks = 0.0f;
        private float _dropTicks = 0.0f;
        private float _dropTimer = 0.1f;

        private StateBase _nextState;
        public Phase1LeapState(ExoArtilleryBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _ticks = 0.0f;

            _oldPos = _entity.transform.position;

            int facingRight = 1;
            if (!_entity._isFacingRight)
                facingRight = -1;
            if (_entity._phase1Count >= 3)
            {
                _nextPos = _entity.transform.position + new Vector3(0.0f, 20.0f);
                _nextState = _entity._damagePhaseState;
            }
            else
            {

                _nextPos = _entity.transform.position + new Vector3(26 * facingRight, 20.0f);
                _nextState = _entity._phase1State;
            }
        }
        public override void Execute()
        {
            _ticks += Time.deltaTime;
            _dropTicks += Time.deltaTime;

            if (_dropTicks >= _dropTimer && _entity._phase1Count < 3)
            {
                _dropTicks = 0.0f;
                _entity.DropParticle(2);
            }

            if (_ticks >= _leapTime)
            {
                if (_entity.IsGrounded())
                {
                    _entity.FlipTo();
                    _entity.SwitchState(_nextState);
                }
            }
            else
                _entity.transform.position = Vector3.Slerp(_oldPos, _nextPos, _ticks / _leapTime);

            

        }
        public override void Exit()
        {
            _entity._phase1Count++;
        }
    }

    public class DamagePhaseState : StateBase
    {
        private float _ticks;
        public DamagePhaseState(ExoArtilleryBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _ticks = 0.0f;
        }
        public override void Execute()
        {
            _ticks += Time.deltaTime;
            if (_ticks >= 10.0f)
            {
                _entity.SwitchState(_entity._phase1LeapState);
            }
        }
        public override void Exit()
        {
            _entity._firstAttack = true;
            _entity._phase1Count = -1;
        }
    }

    public class Phase2State : StateBase
    {
        public Phase2State(ExoArtilleryBehaviour entity) : base(entity) {}
        public void Enter()
        {
        }
        public void Execute()
        {
        }
        public void Exit()
        {

        }
    }
    public class DeathState : StateBase
    {
        public DeathState(ExoArtilleryBehaviour entity) : base(entity) { }
        public void Enter()
        {
            _entity.gameObject.SetActive(false);
        }
        public void Execute()
        {
        }
        public void Exit()
        {

        }
    }

    public void OnDrawGizmos()
    {
        DrawAttack(_stompAttack);
        void DrawAttack(AttackData attack)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(attack.attackCollision.x, attack.attackCollision.y), 
                new Vector3(attack.attackCollision.width, attack.attackCollision.height));
        }
    }
}
