using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase<TEnemy> : EntityStateMachine<TEnemy>, IHittable where TEnemy : EnemyBase<TEnemy>
{
    [System.Serializable]
    protected struct AttackData
    {
        public float moveOffset;
        public Rect attackCollision;
        public int damage;
        public Vector2 knockbackForce;
    }

    protected Rigidbody2D _rb;

    [SerializeField]
    protected EnemyData _enemyData;

    [SerializeField]
    protected int _currentHealth;
    protected int _speed = 1;

    protected Collider2D _visionCollider;

    private Material _mat;

    protected bool _isFacingRight = true;
    protected Vector3 _patrolDirection = Vector3.right;
    public Vector3 PatrolDirection
    {
        get { return _patrolDirection; }
        set { _patrolDirection = value; }
    }

    [Header("Behaviours")] 
    [SerializeField] protected VisionBehaviour _visionBehaviour;
    [SerializeField] protected RangeBehaviour _rangeBehaviour;
    [SerializeField] protected WallDetectBehaviour _wallDetectBehaviour;
    [SerializeField] protected CliffDetectBehaviour _cliffDetectBehaviour;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);
    [SerializeField] LayerMask _groundLayer;

    /** Particle System Test **/
    [SerializeField]
    private GameObject _particleSystem;

    [SerializeField] private int _particleDropsOnHit = 3;
    [SerializeField] private int _particleDropsOnDeath = 7;

    //private PatrolState _patrolState;
    //private DeathState _deathState;

    void DropParticle(int particlesDropped)
    {
        GameObject newObject = Instantiate(_particleSystem, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = newObject.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        float duration = particleSystem.main.duration;

        emission.enabled = true;
        emission.burstCount = particlesDropped;
        particleSystem.Play();
    }

    virtual public void OnHit(Transform source, int damage)
    {
        Stagger(source);
        DropParticle(_particleDropsOnHit);
        _currentHealth -= damage;
        //StartCoroutine(Blink());
        Debug.Log("Hit");
    }

    protected void Stagger(Transform source)
    {
        Vector2 vec = new Vector2(transform.position.x - source.position.x, transform.position.y - source.position.y);
        vec.Normalize();
        _rb.AddForce(vec * 5, ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _groundLayer);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _enemyData.Health;
        _mat = GetComponent<SpriteRenderer>().material;

        //_particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {
        //_patrolState = new PatrolState(this);
        //_deathState = new DeathState(this);
        
        //SwitchState(_patrolState);

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (this._currentHealth <= 0)
        {
            //SwitchState(_deathState);
        }
        Flip();
    }

    virtual protected void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = PatrolDirection.x;
        transform.localScale = localScale;
    }
    /*
    public abstract class StateBase : EntityState<EnemyBase<>>
    {
        public StateBase(EnemyBase entity) : base(entity) { }
    }

    public class PatrolState : StateBase
    {

        protected float _patrolTick;
        public PatrolState(EnemyBase entity) : base(entity) { }
        public void Enter()
        {
            _patrolTick = 0.0f;
        }
        public void Execute()
        {
            Vector3 pos = new(_entity.transform.position.x + (_entity._patrolDirection.x * _entity._speed), _entity.transform.position.y + (_entity._patrolDirection.y * _entity._speed), 0);
            _entity.transform.position = Vector3.MoveTowards(_entity._rb.transform.position, pos, Time.deltaTime * _entity._speed);
        }
        public void Exit()
        {

        }
    }

    public class DeathState : StateBase
    {
        public DeathState(EnemyBase entity) : base(entity) { }
        public void Enter()
        {
            _entity.DropParticle(_entity._particleDropsOnDeath);
            _entity.gameObject.SetActive(false);
        }
        public void Execute()
        {
        }
        public void Exit()
        {

        }
    }*/
}
