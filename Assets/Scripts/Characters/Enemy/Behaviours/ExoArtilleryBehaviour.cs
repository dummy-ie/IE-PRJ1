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
    private Phase2State _phase2State;
    private DeathState _deathState;
    virtual public void OnHit(Transform source, int damage)
    {
    }

    protected virtual void Start()
    {
        _preBattleState = new PreBattleState(this);
        _phase1State = new Phase1State(this);
        
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
        //private MortarState _mortarState;

        public Phase1State(ExoArtilleryBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            //_mortarState = new(_entity);
            //SwitchState(_mortarState);
        }
        

        public override void Exit()
        {
            SwitchState(null);
        }
        public class FirstAttackState : StateBase
        {
            public FirstAttackState(ExoArtilleryBehaviour entity) : base(entity) { }

            public override void Enter()
            {

            }
        }
        public class SecondAttackState : StateBase
        {
            public SecondAttackState(ExoArtilleryBehaviour entity) : base(entity) { }

            public override void Enter()
            {
                
            }
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
