using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gunship;

public class LM40DroneA : LM40DroneBase<LM40DroneA>
{

    [SerializeField]
    private GameObject _bullet;
    
    private ShootState _shootState;
    private SummonState _summonState;


    override protected void Start()
    {
        Debug.Log("AAAAAAAA");
        _shootState = new ShootState(this);
        _summonState = new SummonState(this);

        SwitchState(_shootState);

    }

    public abstract class StateBase : EntityState<LM40DroneA>
    {
        protected StateBase(LM40DroneA entity) : base(entity) { }
    }

    public class ShootState : StateBase
    {

        float shootTicks = .5f;
        float shootCooldown = 1.5f;

        float durationTicks = 0.0f;
        float stateDuration = 6.0f;

        bool arrivedAtLoc = false;

        public ShootState(LM40DroneA entity) : base(entity) { }

        public override void Enter()
        {

            _entity.floatingValueX = 2;
            _entity.floatingValueY = 15;

            int dir = 1;
            if (Random.Range(0, 2) == 1)
            {
                dir = -1;
            }
            Debug.Log(_entity._targetPlayer.transform.position + Vector3.right * 10 * dir);
            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.right * 10 * dir);
        }

        public override void Execute()
        {

            durationTicks += Time.deltaTime;
            shootTicks += Time.deltaTime;

            if (_entity.GetDistanceToTarget() >= 1 && !arrivedAtLoc)
            {
                _entity.Moving();
            }
            else
            {
                arrivedAtLoc = true;
                _entity.Hovering();
            }

            if (shootCooldown <= shootTicks)
            {
                shootTicks = 0;
                GameObject projectile = Instantiate(_entity._bullet, new Vector3(_entity.transform.position.x + 0.2f * _entity.GetDirection(_entity._targetPlayer), _entity.transform.position.y + 0.1f, _entity.transform.position.z), Quaternion.identity);

                // set source and target
                var temp = projectile.GetComponent<DirectionalProjectile>();
                temp.SourcePlayer = _entity.gameObject;

                temp.SetTarget(_entity._targetPlayer.transform);
            }


            if (stateDuration <= durationTicks)
            {

                //_entity.SwitchState(_entity._summonState);

            }

        }

    }

    public class SummonState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 3.0f;

        public SummonState(LM40DroneA entity) : base(entity) { }

        public override void Enter()
        {


        }

        public override void Execute()
        {

            ticks += Time.deltaTime;

            if (stateDuration <= ticks)
            {

                _entity.SwitchState(_entity._summonState);

            }

        }

    }
}
