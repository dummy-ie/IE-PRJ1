using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static LM40DroneA;

public class LM40DroneB : LM40DroneBase<LM40DroneB>
{

    private PrepDashState _prepDashState;
    private DashState _dashState;
    private BuffState _buffState;

    Vector2 _point;
    Vector2 _rndRotation;

    int _dashCount = 0;

    override protected void Start()
    {
        Debug.Log("LM40 DRONE B");
        
        _prepDashState = new PrepDashState(this);
        _dashState = new DashState(this);
        _buffState = new BuffState(this);

        _targetPlayer = GameObject.Find("Player");

        SwitchState(_prepDashState);
    }

    public abstract class StateBase : EntityState<LM40DroneB>
    {
        protected StateBase(LM40DroneB entity) : base(entity) { }
    }


    public class PrepDashState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 3.0f;

        
        public PrepDashState(LM40DroneB entity) : base(entity) { }

        public override void Enter()
        {
            _entity._rndRotation = Random.insideUnitCircle.normalized;
            
            //_entity.MoveTargetLoc(_entity.point);
        }

        public override void Execute()
        {
            _entity._point = new Vector2(_entity._targetPlayer.transform.position.x, _entity._targetPlayer.transform.position.y) + _entity._rndRotation * 10;
            _entity.MoveTargetLoc(_entity._point);

            _entity.SetVelocity(_entity.Moving());
            ticks += Time.deltaTime;

            

            if (stateDuration <= ticks)
            {
                _entity.SwitchState(_entity._dashState);
                ticks = 0;
            }

        }

    }


    public class DashState : StateBase
    {
        float telegraphTick = 0;
        float telegraphDuration = 1f; 

        float ticks = 0.0f;
        float stateDuration = 2.0f;


        public DashState(LM40DroneB entity) : base(entity) { }

        public override void Enter()
        {
            
            telegraphTick = 0;
            _entity._point = new Vector2(_entity._targetPlayer.transform.position.x, _entity._targetPlayer.transform.position.y) + _entity._rndRotation * -10;
            _entity.MoveTargetLoc(_entity._point);
            _entity.SetVelocity(Vector3.zero);
        }

        public override void Execute()
        {
            ticks += Time.deltaTime;
            telegraphTick += Time.deltaTime;
            
            if (telegraphDuration <= telegraphTick)
            {
                _entity.SetVelocity(_entity.Moving() / 1.5f);
            }

            if (stateDuration <= ticks)
            {
                ticks = 0;
                _entity._dashCount++;
                if (_entity._dashCount == 3)
                {
                    _entity.SwitchState(_entity._buffState);
                }
                else
                {
                    _entity.SwitchState(_entity._prepDashState);
                }
                
            }

        }

    }

    public class BuffState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 7.0f;


        public BuffState(LM40DroneB entity) : base(entity) { }

        public override void Enter()
        {
            _entity._dashCount = 0;
            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.right * 20);

            Debug.Log("BUFFFFFF");

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Breakable"))
            {
                IBuffable buff = enemy.GetComponent<IBuffable>();
                if (buff != null)
                {
                    buff.Buff(25, 5);
                }

            }
        }

        public override void Execute()
        {

            ticks += Time.deltaTime;

            _entity.SetVelocity(_entity.Moving()/10);

            if (stateDuration <= ticks)
            {
                _entity.SwitchState(_entity._prepDashState);
                ticks = 0;
            }

        }

        public override void Exit()
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Breakable"))
            {
                IBuffable buff = enemy.GetComponent<IBuffable>();
                if (buff != null)
                {
                    buff.Buff(0, 0);
                }

            }
        }

    }
}
