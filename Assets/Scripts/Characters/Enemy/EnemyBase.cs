using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase<TEnemy> : EntityStateMachine<TEnemy>, IBuffable,IHittable where TEnemy : EnemyBase<TEnemy>
{
    [System.Serializable]
    protected struct AttackData
    {
        public bool drawGizmos;
        public float moveOffset;
        public Rect attackCollision;
        public int damage;
        public Vector2 knockbackForce;
        public bool parriable;
    }

    protected Rigidbody2D rb;
    public Rigidbody2D RB
    {
        get { return rb; }
    }

    [SerializeField]
    protected EnemyData _enemyData;


    [SerializeField]
    protected int _currentHealth;

    [SerializeField]
    protected int _damageBuff;

    protected Collider2D _visionCollider;

    private Material _mat;

    protected int _facingDirection = 1;
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
    [SerializeField] protected LayerMask _groundLayer;

    /** Particle System Test **/
    [SerializeField]
    private GameObject _particleSystem;

    [SerializeField] private int _particleDropsOnHit = 3;
    [SerializeField] private int _particleDropsOnDeath = 7;

    [Header("Pathfinding")]
    public float PathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float Speed = 100f, JumpForce = 100f;
    public float NextWaypointDistance = 3f;
    public float JumpNodeHeightRequirement = 0.8f;
    public float JumpModifier = 0.3f;
    public float JumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool FollowEnabled = true;
    public bool JumpEnabled = true, IsJumping, IsInAir;
    public bool DirectionLookEnabled = true;

    protected Path path;
    protected int currentWaypoint = 0;
    protected Seeker seeker;

    protected bool isJumpOnCooldown = false;

    //private PatrolState _patrolState;
    //private DeathState _deathState;

    public void DropParticle(int particlesDropped)
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
        //Stagger(source);
        DropParticle(_particleDropsOnHit);
        _currentHealth -= damage;
        //StartCoroutine(Blink());
        Debug.Log("Hit");
    }

    protected void Stagger(Transform source)
    {
        Vector2 vec = new Vector2(transform.position.x - source.position.x, transform.position.y - source.position.y);
        vec.Normalize();
        rb.AddForce(vec * 5, ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _groundLayer);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _currentHealth = _enemyData.Health;
        //_mat = GetComponent<SpriteRenderer>().material;

        //_particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    virtual protected void FlipTo(float targetPosition)
    {
        float posX = rb.position.x;
        if ((posX < targetPosition && _facingDirection == -1) || (posX > targetPosition && _facingDirection == 1))
            Flip(_facingDirection * -1);
    }

    virtual protected void Flip(int direction)
    {
        _facingDirection = direction;
        Vector3 scale = transform.localScale;
        if ((scale.x < 0 && _facingDirection == 1) || (scale.x > 0 && _facingDirection == -1))
            scale.x *= -1;
        transform.localScale = scale;
    }

    virtual public void Buff(int healthBuff, int damageBuff)
    {
        this._currentHealth += healthBuff;
        this._damageBuff = damageBuff;
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected IEnumerator JumpCooldown()
    {
        isJumpOnCooldown = true;
        yield return new WaitForSeconds(1f);
        isJumpOnCooldown = false;
    }
}
