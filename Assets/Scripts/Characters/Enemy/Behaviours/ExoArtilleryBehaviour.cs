using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Rigidbody2D))]
public class ExoArtilleryBehaviour : EnemyBase<ExoArtilleryBehaviour>
{

    [SerializeField] AttackData _attack;

    private PreBattleState _preBattleState;
    private Phase1State _phase1State;
    private Phase1LeapState _phase1LeapState;
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
        
        SwitchState(_phase1State);
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
    }

    public class Phase1State : StateBase
    {
        private float _stompTicks = 0.0f;
        private float _mortarTicks = 0.0f;
        private float _elapsedTime = 0.0f;
        public Phase1State(ExoArtilleryBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            _stompTicks = 0.0f;
            _mortarTicks = 0.0f;
            _elapsedTime = 0.0f;
        }

        public override void Execute()
        {
            _stompTicks += Time.deltaTime;
            _mortarTicks += Time.deltaTime;
            _elapsedTime += Time.deltaTime;

            if (_stompTicks >= 3.0f)
            {
                _stompTicks = 0.0f;
                PerformStomp();
            }

            if (_mortarTicks >= 1.0f)
            {
                PerformMortar();
            }

            if (_elapsedTime >= 9.0f)
            {
                _entity.SwitchState(_entity._phase1LeapState);
            }
        }
        public override void Exit()
        {
            //SwitchState(null);
        }

        private void PerformStomp()
        {
            int facingRight = 1;
            if (!_entity._isFacingRight)
                facingRight = -1;
            _entity.transform.position += new Vector3(2.0f * facingRight, 0);
        }

        private void PerformMortar()
        {

        }
    }

    public class Phase1LeapState : StateBase
    {
        private Vector3 _oldPos;
        private Vector3 _nextPos;
        private float _leapTime = 7.0f;
        private float _ticks = 0.0f;
        public Phase1LeapState(ExoArtilleryBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            _ticks = 0.0f;

            _oldPos = _entity.transform.position;

            int facingRight = 1;
            if (!_entity._isFacingRight)
                facingRight = -1;
            _nextPos = _entity.transform.position + new Vector3(6 * facingRight, 20.0f);
        }
        public override void Execute()
        {
            _ticks += Time.deltaTime;
            
            if (_ticks >= _leapTime)
            {
                if (_entity.IsGrounded())
                {
                    _entity.FlipTo();
                    _entity.SwitchState(_entity._phase1State);
                }

            }
            else
                _entity.transform.position = Vector3.Slerp(_oldPos, _nextPos, _ticks / _leapTime);

        }
        public override void Exit()
        {

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
        }
        public void Execute()
        {
        }
        public void Exit()
        {

        }
    }
}
