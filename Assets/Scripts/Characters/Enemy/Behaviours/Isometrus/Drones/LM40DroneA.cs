using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gunship;

public class LM40DroneA : LM40DroneBase<LM40DroneA>
{

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private GameObject _testEnemy;
    
    private ShootState _shootState;
    private SummonState _summonState;

    int dir = 1;

    override protected void Start()
    {
        Debug.Log("LM40 DRONE A");
        _shootState = new ShootState(this);
        _summonState = new SummonState(this);

        _targetPlayer = GameObject.Find("Player");

        SwitchState(_shootState);

    }

    public abstract class StateBase : EntityState<LM40DroneA>
    {
        protected StateBase(LM40DroneA entity) : base(entity) { }
    }

    public void StartShootingBullets(int count) { StartCoroutine(ShootBullet(count)); }
    private IEnumerator ShootBullet(int count)
    {
        GameObject projectile = Instantiate(_bullet, new Vector3(transform.position.x * GetDirection(_targetPlayer), transform.position.y, transform.position.z), Quaternion.identity);
        rb.AddForce((_targetPlayer.transform.position - transform.position) * 100);
        // set source and target
        var temp = projectile.GetComponent<DirectionalProjectile>();
        temp.SourcePlayer = gameObject;

        temp.SetTarget(_targetPlayer.transform);
        count--;
        yield return new WaitForSeconds(.1f);

        if (count > 0) { StartCoroutine(ShootBullet(count)); }
    }

    public class ShootState : StateBase
    {

        float shootTicks = .5f;
        float shootCooldown = 1.5f;

        float durationTicks = 0.0f;
        float stateDuration = 6.0f;

        public ShootState(LM40DroneA entity) : base(entity) { }

        public override void Enter()
        {

            _entity.floatingValueX = 2;
            _entity.floatingValueY = 15;

            //Debug.Log(_entity._targetPlayer.transform.position + Vector3.right * 10 * dir);
            
        }

        public override void Execute()
        {

            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.right * 10 * _entity.dir);

            durationTicks += Time.deltaTime;
            shootTicks += Time.deltaTime;


            _entity.SetVelocity(_entity.Hovering() + _entity.Moving());


            if (shootCooldown <= shootTicks)
            {
                shootTicks = 0;
                _entity.StartShootingBullets(5);
            }


            if (stateDuration <= durationTicks)
            {

                _entity.SwitchState(_entity._summonState);
                durationTicks = 0;
            }

        }

    }

    public class SummonState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 5.0f;

        bool hasSpawned = false;

        public SummonState(LM40DroneA entity) : base(entity) { }

        public override void Enter()
        {
            hasSpawned = false;

            
            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.right * 15 * _entity.dir);
        }

        public override void Execute()
        {

            ticks += Time.deltaTime;

            _entity.SetVelocity(_entity.Moving()/2);

            if (stateDuration / 2 <= ticks && !hasSpawned)
            {
                GameObject projectile = Instantiate(_entity._testEnemy, _entity.transform.position, Quaternion.identity);
                hasSpawned = true;
            }

            if (stateDuration <= ticks)
            {
                _entity.SwitchState(_entity._shootState);
                ticks = 0;
            }

        }

        public override void Exit()
        {
            _entity.dir *= -1;
        }
    }
}
