
using UnityEngine;

using static UnityEngine.EventSystems.EventTrigger;

public class XD1Behaviour : EntityStateMachine<XD1Behaviour>
{
    [SerializeField] private float _acceleration;
    [SerializeField] private Vector3 _offsetFromPlayer;
    [SerializeField] private float _catchUpTime = 0.4f;
    private GameObject _player;
    private Vector3 _velocity;
    private Rigidbody _rb;

    private Material _material;

    private IdleState _idleState;
    private FollowState _followState;


    public void CollectManite()
    {
        if (_player.CompareTag("Player"))
        {
            CharacterController2D controller = _player.GetComponent<CharacterController2D>();

            if (controller.Stats.Manite.Current <= controller.Stats.Manite.Max)
            {
                controller.Stats.Manite.Current += 1;
            }
            else
                controller.Stats.Manite.Current = controller.Stats.Manite.Max;
        }
    }

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();

        _idleState = new IdleState(this);
        _followState = new FollowState(this);

        SwitchState(_idleState);
    }

    protected void Update()
    {
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");
    }

    public abstract class StateBase : EntityState<XD1Behaviour>
    {
        protected StateBase(XD1Behaviour entity) : base(entity) {}
    }

    public class IdleState : StateBase
    {
        public IdleState(XD1Behaviour entity) : base(entity) {}

        public override void Enter()
        {
            Debug.Log("Entered Idle State");
        }
        public override void Execute()
        {
            if (Vector3.Distance(_entity.transform.position, _entity._player.transform.position) >= 2)
            {
                Debug.Log("idle");
                _entity.SwitchState(_entity._followState);
            }
        }
    }

    public class FollowState : EntityState<XD1Behaviour>
    {
        Vector3 _target;
        private float _followTicks;
        public FollowState(XD1Behaviour entity) : base(entity) { }
        public override void Enter()
        {
            _followTicks = 0.0f;
            
        }

        public override void Execute()
        {
            if (_entity._player.GetComponent<CharacterController2D>().FacingDirection == 1)
            {
                _target = new Vector3(-_entity._offsetFromPlayer.x, _entity._offsetFromPlayer.y, _entity._offsetFromPlayer.z) + _entity._player.transform.position;
            }
            else
            {
                _target = _entity._offsetFromPlayer + _entity._player.transform.position;
            }
            _entity.transform.position = Vector3.SmoothDamp(_entity.transform.position, _target, ref _entity._velocity, _entity._catchUpTime);
            if (_entity._velocity.magnitude <= 1f)
            {
                _followTicks += Time.deltaTime;
                if (_followTicks > 2)
                    _entity.SwitchState(_entity._idleState);
            }
            else
            {
                _followTicks = 0;
            }
            Vector3 lookAtPosition = new Vector3(_entity._player.transform.position.x, _entity.transform.position.y, _entity.transform.position.z);
            _entity.transform.LookAt(lookAtPosition, Vector3.up);
        }
    }
}
