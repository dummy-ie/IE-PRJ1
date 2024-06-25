
using UnityEngine;
using Pathfinding;
using static UnityEngine.EventSystems.EventTrigger;

public class GruntBehavior : EnemyBase<GruntBehavior>
{
    [SerializeField] AttackData _attack;

    [SerializeField] public GameObject _directionalCannon;

    private GameObject _playerTarget;

    [SerializeField] private GameObject _patrolTarget;

    [SerializeField] private GameObject _patrolTarget2;

    private IdleState _idleState;
    private DeathState _deathState;
    private PatrolState _patrolState;
    private ActivatedState _activeState;
    private MoveState _moveState;
    private FollowTargetState _followTargetState;
    private ShootState _shootState;

    public void ActivateBoss(){
        SwitchState(_activeState);
    }
    
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

    public void SetPatrolTarget(GameObject position, int nPoint) {
        if(nPoint == 1){
            _patrolTarget = position;
        }

        else {
            _patrolTarget2 = position;
        }
    }

    public void SetTarget(GameObject target){
        this._playerTarget = target;
    }

    private bool ShouldFollowTarget() => _visionBehaviour.PlayerSeen && FollowEnabled;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        IsJumping = false;
        IsInAir = false;
        isJumpOnCooldown = false;

        _playerTarget = GameObject.FindGameObjectWithTag("Player");

        _idleState = new IdleState(this);
        _deathState = new DeathState(this);
        _patrolState = new PatrolState(this);
        _shootState = new ShootState(this);
        _moveState = new MoveState(this);
        _followTargetState = new FollowTargetState(this);
        _activeState = new ActivatedState(this);

        InvokeRepeating("UpdatePath", 0f, PathUpdateSeconds);

        SwitchState(_idleState);
    }

    private void UpdatePath()
    {
        if (ShouldFollowTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, _playerTarget.transform.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * Speed;

        if (JumpEnabled && IsGrounded() && !IsInAir && !isJumpOnCooldown)
        {
            if (direction.y > JumpNodeHeightRequirement)
            {
                IsJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                StartCoroutine(JumpCooldown());

            }
        }

        if (IsGrounded())
        {
            IsJumping = false;
            IsInAir = false;
        }
        else
        {
            IsInAir = true;
        }

        rb.velocity = new Vector2(force.x, rb.velocity.y);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < NextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (DirectionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    

    public abstract class StateBase : EntityState<GruntBehavior>
    {
        protected StateBase(GruntBehavior entity) : base(entity) { }
    }

    public class IdleState: StateBase {

        public IdleState(GruntBehavior entity) : base(entity) { }
        public override void Execute()
        {
            if(_entity._visionBehaviour.PlayerSeen == false){

                _entity.SwitchState(_entity._moveState);

            }
        }
    }

    public class ActivatedState : StateBase
    {
         float ticks = 0.0f;

        float TickInterval = 5.0f;
        public ActivatedState(GruntBehavior entity) : base(entity) { }

        public override void Execute()
        {
            ticks += 0.05f;

            _entity.CheckHealth();

            _entity.SwitchState(_entity._patrolState);
        }   

    }

    public class ShootState : StateBase {

        int nNumberofProjectiles = 0;
        float ticks = 0.0f;

        float TickInterval = 5.0f;

        public ShootState(GruntBehavior entity) : base(entity) { }

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

                    _entity.SwitchState(_entity._activeState);
                    ticks = 0.0f;

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

        public PatrolState(GruntBehavior entity) : base(entity) { }

        public override void Execute() {

            _entity.CheckHealth();

            if(_entity._visionBehaviour.PlayerSeen == false) {
                //_entity.SwitchState(_entity._moveState);
            }
            // pathfinding prototype
            else if (_entity.ShouldFollowTarget())
            {
                _entity.SwitchState(_entity._followTargetState);
            }


            /*else{
                _entity.SwitchState(_entity._shootState);
            }*/
        }

    }

     // TODO : move this state to patrol state so it can follow target if it sees while moving
    public class MoveState : StateBase{

        bool goToPointB = false;
        float fTimeInterval = 150.0f;

        float fTicks = 0.0f;

        public MoveState(GruntBehavior entity) : base(entity) {}

        public override void Execute(){

            _entity.CheckHealth();

            if(goToPointB == false){
                    Vector2 pos = new(_entity.transform.position.x - (_entity.GetDirection(_entity._patrolTarget)/* * _entity.Speed*/), 0.0f);
                    _entity.transform.position = Vector2.MoveTowards(_entity.rb.transform.position, pos, Time.deltaTime * _entity.Speed);

                    if(fTicks >= fTimeInterval){
                        fTicks = 0.0f;
                        goToPointB = true;
                    }

                fTicks += 0.05f;

                _entity.SwitchState(_entity._patrolState);
            }
                    
            else if(goToPointB == true){

                Vector2 pos2 = new(_entity.transform.position.x + (_entity.GetDirection(_entity._patrolTarget2)/* * _entity.Speed*/), 34.0f);
                _entity.transform.position = Vector2.MoveTowards(_entity.rb.transform.position, pos2, Time.deltaTime * _entity.Speed);

                if(fTicks >= fTimeInterval){
                    fTicks = 0.0f;
                    goToPointB = false;
                }

                fTicks += 0.05f;

                _entity.SwitchState(_entity._patrolState);
            }
        }
    }

    public class FollowTargetState : StateBase
    {

        bool goToPointB = false;
        float fTimeInterval = 150.0f;

        float fTicks = 0.0f;

        public FollowTargetState(GruntBehavior entity) : base(entity) { }

        public override void Enter()
        {
            Debug.LogWarning("FOLLOWING TARGET");
        }

        public override void Execute()
        {
            _entity.PathFollow();
            if (!_entity.ShouldFollowTarget())
            {
                Debug.LogWarning("stio folginwfing");
                _entity.SwitchState(_entity._idleState);
                return;
            }
        }
    }

    public class DeathState : StateBase {

        public DeathState(GruntBehavior entity) : base(entity) { }

        public override void Execute() {
            
            Destroy(this._entity.gameObject);

        }

    }

}


