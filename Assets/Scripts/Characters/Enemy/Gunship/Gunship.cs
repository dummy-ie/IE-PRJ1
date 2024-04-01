
using UnityEngine;

public class Gunship : EnemyBase<Gunship>
{
    [SerializeField] AttackData _attack;

    [SerializeField] public GameObject _directionalEMP;

    [SerializeField] public GameObject _directionalCannon;

    [SerializeField] private GameObject _EMPtarget;

    private GameObject _playerTarget;

    [SerializeField] private GameObject _patrolObject;

    [SerializeField] private GameObject _patrolObject2;

    private DeathState _deathState;
    private PatrolState _patrolState;
    private ActivatedState _activeState;
    private MoveState _moveState;
    private ShootState _shootState;
    
    public void CheckHealth(){
        if(_currentHealth <= 0){
            SwitchState(_deathState);
        }
    }

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

    public void SetPatrolTarget(GameObject target, int nPoint){

        if(nPoint == 1){
            _patrolObject = target;
        }

        else {
            _patrolObject2 = target;
        }
        
    }

    

    public void SetTarget(GameObject target){
        this._playerTarget = target;
    }

    override protected void Start()
    {
        
        _deathState = new DeathState(this);
        _patrolState = new PatrolState(this);
        _shootState = new ShootState(this);
        _moveState = new MoveState(this);
        _activeState = new ActivatedState(this);

        SwitchState(_activeState);

    }

    public abstract class StateBase : EntityState<Gunship>
    {
        protected StateBase(Gunship entity) : base(entity) { }
    }

    public class ActivatedState : StateBase
    {
         float ticks = 0.0f;

        float TickInterval = 5.0f;
        public ActivatedState(Gunship entity) : base(entity) { }

        public override void Execute()
        {
            ticks += 0.05f;

            _entity.CheckHealth();

                if(TickInterval <= ticks) {
            
                    GameObject projectile = Instantiate(_entity._directionalEMP, new Vector3(_entity.transform.position.x + 0.2f * _entity.GetDirection(_entity._EMPtarget), _entity.transform.position.y + 0.1f, _entity.transform.position.z), Quaternion.identity);

                    // set source and target
                    var temp = projectile.GetComponent<DirectionalProjectile>();
                    temp.SourcePlayer = _entity.gameObject;

                    temp.SetTarget(_entity._EMPtarget.transform);

                    _entity.SwitchState(_entity._patrolState);

                    ticks = 0.0f;
                }
        }   

    }

    public class ShootState : StateBase {

        int nNumberofProjectiles = 0;
        float ticks = 0.0f;

        float TickInterval = 5.0f;

        public ShootState(Gunship entity) : base(entity) { }

        public override void Enter()
        {

            
        }

        public override void Execute()
        {
            _entity.CheckHealth();

            ticks += 0.05f;

            if(TickInterval <= ticks) {
        
                if(_entity._visionBehaviour.PlayerSeen == true && _entity._rangeBehaviour.InRange == true){
                
                    GameObject projectile = Instantiate(_entity._directionalCannon, new Vector3(_entity.transform.position.x + 0.2f * _entity.GetDirection(_entity._playerTarget), _entity.transform.position.y + 0.1f, _entity.transform.position.z), Quaternion.identity);

                    // set source and target
                    var temp = projectile.GetComponent<DirectionalProjectile>();
                    temp.SourcePlayer = _entity.gameObject;
                    _entity._playerTarget = _entity._visionBehaviour.detectedObject;

                    temp.SetTarget(_entity._playerTarget.transform);

                    nNumberofProjectiles += 1;

                    if(nNumberofProjectiles >= 4){

                        _entity.SwitchState(_entity._activeState);
                        ticks = 0.0f;
                    }

                //_entity.SwitchState(this);

                }

                else if(_entity._visionBehaviour.PlayerSeen == false) {
                    _entity.SwitchState(_entity._patrolState); //switch to patrol state
                    ticks = 0.0f;
                }

                else {
                    _entity.SwitchState(_entity._patrolState); 
                    ticks = 0.0f;
                }

            }
          
        }

    }

     public class PatrolState : StateBase {

        bool goToPointB = false;

        public PatrolState(Gunship entity) : base(entity) { }

        public override void Execute() {

            _entity.CheckHealth();

            if(_entity._visionBehaviour.PlayerSeen == false){

                _entity.SwitchState(_entity._moveState);

            }

            else{
                _entity.SwitchState(_entity._shootState);
            }
        }

    }

    public class MoveState : StateBase{

        bool goToPointB = false;

        public MoveState(Gunship entity) : base(entity) {}

        public override void Execute(){

            _entity.CheckHealth();

            if(goToPointB == false){
                    Vector2 pos = new(_entity.transform.position.x - (_entity.GetDirection(_entity._patrolObject) * _entity._speed), 34.0f);
                    _entity.transform.position = Vector2.MoveTowards(_entity._rb.transform.position, pos, Time.deltaTime * _entity._speed);

                    if(_entity.transform.position.x <= _entity._patrolObject.transform.position.x){
                        goToPointB = true;
                    }

                _entity.SwitchState(_entity._patrolState);
            }
                    
            else if(goToPointB == true){

                Vector2 pos2 = new(_entity.transform.position.x + (_entity.GetDirection(_entity._patrolObject2) * _entity._speed), 34.0f);
                _entity.transform.position = Vector2.MoveTowards(_entity._rb.transform.position, pos2, Time.deltaTime * _entity._speed);

                if(_entity.transform.position.x >= _entity._patrolObject2.transform.position.x){
                    goToPointB = false;
                }

                _entity.SwitchState(_entity._patrolState);
            }
        }
    }

    public class DeathState : StateBase {

        public DeathState(Gunship entity) : base(entity) { }

        public override void Execute() {

            //Destroy(this.gameObject);

        }

    }

}


