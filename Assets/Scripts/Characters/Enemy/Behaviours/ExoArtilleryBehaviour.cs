using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Rigidbody2D))]
public class ExoArtilleryBehaviour : EnemyBase<ExoArtilleryBehaviour>
{

    [SerializeField] AttackData _attack;

    private PreBattleState _preBattleState;
    private MortarState _mortarState;
    virtual public void OnHit(Transform source, int damage)
    {
    }

    protected virtual void Start()
    {
        _preBattleState = new PreBattleState(this);
        
        SwitchState(_preBattleState);
    }

    public abstract class StateBase : EntityState<ExoArtilleryBehaviour>
    {
        public StateBase(ExoArtilleryBehaviour entity) : base(entity) {}
    }

    public class PreBattleState : StateBase
    {
        public PreBattleState(ExoArtilleryBehaviour entity) : base(entity) {}
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
    public class IdleState : StateBase
    {

        public IdleState(ExoArtilleryBehaviour entity) : base(entity) {}
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

    public class MortarState : StateBase
    {
        public MortarState(ExoArtilleryBehaviour entity) : base(entity) {}
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
