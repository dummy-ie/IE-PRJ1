using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LM40DroneA;
using static LM40DroneB;

public class LM40DroneC : LM40DroneBase<LM40DroneC>
{

    private ShootState _shootState;
    private ShockwaveState _shockwaveState;

    [SerializeField]
    private GameObject _beam;

    override protected void Start()
    {
        Debug.Log("LM40 DRONE C");

        _shootState = new ShootState(this);
        _shockwaveState = new ShockwaveState(this);

        _targetPlayer = GameObject.Find("Player");

        SwitchState(_shootState);
    }

    public abstract class StateBase : EntityState<LM40DroneC>
    {
        protected StateBase(LM40DroneC entity) : base(entity) { }
    }

    public class ShootState : StateBase
    {

        float shootingTick=0.0f;
        float shootingDuration = 2;

        float shootTicks = .5f;
        float shootCooldown = 5f;
        

        float durationTicks = 0.0f;
        float stateDuration = 6.0f;

        bool hasShot = false;

        public ShootState(LM40DroneC entity) : base(entity) { }

        public override void Enter()
        {
            _entity.floatingValueX = 1;
            _entity.floatingValueY = 1;
        }

        public override void Execute()
        {

            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + (Vector3.up * 6));

            durationTicks += Time.deltaTime;
            shootTicks += Time.deltaTime;




            

            if (shootCooldown <= shootTicks)
            {

                shootingTick += Time.deltaTime;

                if (!hasShot)
                {
                    GameObject projectile = Instantiate(_entity._beam, // Beam prefab
                                                        _entity.transform.position, // spawn point
                                                        Quaternion.identity); // rotate (handled by projectile direction)

                    var temp = projectile.GetComponentInChildren<LaserProjectile>(); // get in CHILD components
                    temp.SourcePlayer = _entity.gameObject;

                    temp.SetDirection(Vector3.down); // direction will be normalized to 4 cardinal directions. 
                                                     // pass Vector3.up for north, down for south, right for east, left for west.
                                                     // (i'm multiplying this one by -1 if the player is to the left but isometrus has set positions anyway so just pass the 4 cardinal directions maybe [or dont])
                    hasShot = true;
                }

                _entity.SetVelocity(Vector3.zero);
                if (shootingDuration <= shootingTick)
                {
                    shootTicks = 0;
                    shootingTick = 0;
                    hasShot = false;
                }
            }
            else
            {
                
                _entity.SetVelocity((_entity.Moving() + _entity._targetPlayer.GetComponent<Rigidbody2D>().velocity * 2.5f ) + _entity.Hovering() * 1.5f);
            }


            if (stateDuration <= durationTicks)
            {

                //_entity.SwitchState(_entity._shockwaveState);
                durationTicks = 0;
            }

        }

    }


    public class ShockwaveState : StateBase
    {

        float durationTicks = 0.0f;
        float stateDuration = 6.0f;



        public ShockwaveState(LM40DroneC entity) : base(entity) { }

        public override void Enter()
        {
            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.up * 10);
        }

        public override void Execute()
        {

            

            durationTicks += Time.deltaTime;

            _entity.SetVelocity(_entity.Moving());



            if (stateDuration <= durationTicks)
            {

                _entity.SwitchState(_entity._shockwaveState);
                durationTicks = 0;
            }

        }

    }
}
