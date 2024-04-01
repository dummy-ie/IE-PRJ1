using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunship : EnemyBase<Gunship>
{
    [SerializeField] AttackData _attack;

    [SerializeField] public GameObject _directionalEMP;

    [SerializeField] public GameObject _directionalCannon;

    [SerializeField] private GameObject _EMPtarget;

    private GameObject _playerTarget;

    private DeathState _deathState;
    private PatrolState _patrolState;
    private ActivatedState _activeState;
    private ShootState _shootState;
    

    public float GetDirection(GameObject target)
    {
        if(target != null){

            float value = target.transform.position.x - target.transform.position.x;
            if (value < 0)
            return -1;

            else
            return 1;

        }

        return 0;
        
    }

    public void SetTarget(GameObject target){
        this._playerTarget = target;
    }

    override protected void Start()
    {
        
        _deathState = new DeathState(this);
        _patrolState = new PatrolState(this);
        _shootState = new ShootState(this);
        _activeState = new ActivatedState(this);

        SwitchState(_activeState);

    }

    public abstract class StateBase : EntityState<Gunship>
    {
        protected StateBase(Gunship entity) : base(entity) { }
    }

    public class ActivatedState : StateBase
    {

        public ActivatedState(Gunship entity) : base(entity) { }

        public override void Execute()
        {
            GameObject projectile = Instantiate(_entity._directionalEMP, new Vector3(_entity.transform.position.x + 0.2f * _entity.GetDirection(_entity._EMPtarget), _entity.transform.position.y + 0.1f, _entity.transform.position.z), Quaternion.identity);

            // set source and target
            var temp = projectile.GetComponent<DirectionalProjectile>();
            temp.SourcePlayer = _entity.gameObject;

            temp.SetTarget(_entity._EMPtarget.transform);

           _entity.SwitchState(_entity._shootState);
        }

    }

    public class ShootState : StateBase {

        public ShootState(Gunship entity) : base(entity) { }

        public override void Enter()
        {

            
        }

        public override void Execute()
        {
            if(_entity._visionBehaviour.PlayerSeen == true && _entity._rangeBehaviour.InRange == true){
                
                GameObject projectile = Instantiate(_entity._directionalCannon, new Vector3(_entity.transform.position.x + 0.2f * _entity.GetDirection(_entity._playerTarget), _entity.transform.position.y + 0.1f, _entity.transform.position.z), Quaternion.identity);

                // set source and target
                var temp = projectile.GetComponent<DirectionalProjectile>();
                temp.SourcePlayer = _entity.gameObject;
                _entity._playerTarget = _entity._visionBehaviour.detectedObject;

                temp.SetTarget(_entity._playerTarget.transform);

                //_entity.SwitchState(this);

            }

            // else {
            //     _entity.SwitchState(_entity._patrolState); //switch to patrol state
            // }
          
        }

    }

     public class PatrolState : StateBase {

         public PatrolState(Gunship entity) : base(entity) { }

    }

    public class DeathState : StateBase {

         public DeathState(Gunship entity) : base(entity) { }

    }
    

}


