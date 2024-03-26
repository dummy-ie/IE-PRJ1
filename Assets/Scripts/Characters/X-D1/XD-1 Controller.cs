using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class XD1Controller : MonoBehaviour
{
    private StateMachine _stateMachine;

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

        _stateMachine = new StateMachine();
        _idleState = new IdleState(this);
        _followState = new FollowState(this);

        _stateMachine.ChangeState(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
    }


    public class IdleState : IState
    {
        private XD1Controller _controller;
        public IdleState(XD1Controller controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            if (Vector3.Distance(_controller.transform.position, _controller._player.transform.position) >= 2)
            {
                _controller._stateMachine.ChangeState(_controller._followState);
            }
        }
    }

    public class FollowState : IState
    {
        private XD1Controller _controller;
        Vector3 _target;
        private float _followTicks;
        public FollowState(XD1Controller controller)
        {
            _controller = controller;
        }
        public void Enter()
        {
            _followTicks = 0.0f;
            
        }

        public void Execute()
        {
            if (_controller._player.GetComponent<CharacterController2D>().FacingDirection == 1)
            {
                _target = new Vector3(-_controller._offsetFromPlayer.x, _controller._offsetFromPlayer.y, _controller._offsetFromPlayer.z) + _controller._player.transform.position;
            }
            else
            {
                _target = _controller._offsetFromPlayer + _controller._player.transform.position;
            }
            _controller.transform.position = Vector3.SmoothDamp(_controller.transform.position, _target, ref _controller._velocity, _controller._catchUpTime);
            if (_controller._velocity.magnitude <= 1f)
            {
                _followTicks += Time.deltaTime;
                if (_followTicks > 2)
                    _controller._stateMachine.ChangeState(_controller._idleState);
            }
            else
            {
                _followTicks = 0;
            }
            Vector3 lookAtPosition = new Vector3(_controller._player.transform.position.x, _controller.transform.position.y, _controller.transform.position.z);
            _controller.transform.LookAt(lookAtPosition, Vector3.up);
        }
    }
}
