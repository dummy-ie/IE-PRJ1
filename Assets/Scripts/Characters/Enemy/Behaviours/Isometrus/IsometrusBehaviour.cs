using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LM40DroneC;

public class IsometrusBehaviour : LM40DroneBase<IsometrusBehaviour>, IBuffable, IHittable
{
    private StasisState _statisState;
    private YappingState _yappingState;
    private BasicState _basicState;

    
    [SerializeField] GameObject _droneA;
    [SerializeField] GameObject _droneB;
    [SerializeField] GameObject _droneC;
    bool _droneASpawned = false, _droneBSpawned = false, _droneCSpawned = false;
    GameObject _droneAobj;
    GameObject _droneBobj;
    GameObject _droneCobj;

    // TEMPORARY
    private int _maxHealth;
    [SerializeField] private BossBar _bossBar;
    [SerializeField] private GameObject _results;

    override protected void Start()
    {
        Debug.Log("ISOMETRUSSY");

        _statisState = new(this);
        _yappingState = new(this);
        _basicState = new BasicState(this);

        _maxHealth = _currentHealth;
    }

    // TEMPORARY
    protected override void Update()
    {
        if (_currentHealth <= 0)
            _results.SetActive(true);
        _bossBar.SetHealth(_currentHealth, _maxHealth);
        base.Update();
    }

    public void TriggerEncounter()
    {
        _bossBar.gameObject.SetActive(true); // TEMPORARY
        SwitchState(_yappingState);
    }

    public abstract class StateBase : EntityState<IsometrusBehaviour>
    {
        protected StateBase(IsometrusBehaviour entity) : base(entity) { }
    }

    public class StasisState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 3.0f;


        public StasisState(IsometrusBehaviour entity) : base(entity) { }

        public override void Enter()
        {
           
        }

        public override void Execute()
        {


            if (stateDuration <= ticks)
            {
                _entity.SwitchState(_entity._basicState);
                ticks = 0;
            }

        }

        

    }

    public class YappingState : StateBase
    {

        float ticks = 0.0f;

        float stateDuration = 3.0f;


        public YappingState(IsometrusBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            _entity._targetPlayer.GetComponent<CharacterController2D>().CanMove = false;
            _entity._targetPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + (Vector3.right * 10) + (Vector3.up * 5));

        }

        public override void Execute()
        {
            ticks+= Time.deltaTime;

            _entity.SetVelocity(_entity.Moving()/2);

            if (stateDuration <= ticks)
            {
                _entity.SwitchState(_entity._basicState);
                ticks = 0;
            }

        }
        public override void Exit()
        {
            _entity._targetPlayer.GetComponent<CharacterController2D>().CanMove = true;
        }
    }

    public class BasicState : StateBase
    {
        float ticks = 0.0f;

        float stateDuration = 3.0f;

        float moveTicks = 0.0f;
        float moveIntervals = 5;

        int dir = 1;

        public BasicState(IsometrusBehaviour entity) : base(entity) { }

        public override void Enter()
        {
            
            _entity._droneAobj = Instantiate(_entity._droneA, _entity.transform.position + new Vector3(1, 1) * 15, Quaternion.identity);
            _entity._droneASpawned = true;
        }

        public override void Execute()
        {
            
            _entity.SetVelocity(_entity.Hovering() + (_entity.Moving()/2));

            moveTicks += Time.deltaTime;




            if (moveIntervals < moveTicks)
            {
                moveTicks = 0;

                if (_entity._targetPlayer.transform.position.x < _entity.transform.position.x) dir = 1;
                else dir = -1;
                _entity.MoveTargetLoc(_entity._targetPlayer.transform.position + Vector3.right * 5 * dir);
            }

        }

    }


    [SerializeField]
    protected int _damageBuff;

    void CheckForDroneSpawn()
    {
        if (_currentHealth <= 70 & !_droneBSpawned)
        {
            _droneBobj = Instantiate(_droneB, transform.position + new Vector3(1,1) * 15, Quaternion.identity);
            _droneBSpawned = true;
        }
        if (_currentHealth <= 40 & !_droneCSpawned)
        {
            _droneCobj = Instantiate(_droneC, transform.position + new Vector3(1, 1) * 15, Quaternion.identity);
            _droneCSpawned = true;
        }
    }

    virtual public void OnHit(Transform source, int damage)
    {
        //Stagger(source);
        //DropParticle(_particleDropsOnHit);
        _currentHealth -= damage;
        //StartCoroutine(Blink());

        if (_currentHealth <= 0)
        {
            Destroy(_droneAobj);
            Destroy(_droneBobj);
            Destroy(_droneCobj);
            Destroy(this.gameObject);
        }

        CheckForDroneSpawn();

        Debug.Log("Hit");
        Debug.Log("Damage Dealt: " + damage);
        Debug.Log("Current Health: " + _currentHealth);
    }

    virtual public void Buff(int healthBuff, int damageBuff)
    {
        this._currentHealth += healthBuff;
        this._damageBuff = damageBuff;
    }
}
