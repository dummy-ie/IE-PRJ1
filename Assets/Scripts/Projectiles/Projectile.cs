using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected ProjectileData _data;

    protected float _lifespanCounter = 0;

    GameObject _sourcePlayer;
    public GameObject SourcePlayer
    {
        get { return _sourcePlayer; }
        set { _sourcePlayer = value; }
    }

    protected abstract void Move();

    // The method to run once the projectile's lifespan expires. By default, it destroys the gameObject.
    protected virtual void OnExpire() => Destroy(gameObject);

    protected virtual float CalculateSpeed() => _data.Speed;

    protected int CheckFlipped()
    {
        if (transform.localScale.y < 0)
            return 1;

        return -1;
    }

    protected void FixedUpdate()
    {
        if (_lifespanCounter <= _data.LifespanTime)
            Move();
    }

    protected void Update()
    {
        _lifespanCounter += Time.deltaTime;

        if (_lifespanCounter > _data.LifespanTime)
            OnExpire();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (_sourcePlayer.CompareTag("Player") && other.gameObject.CompareTag("Breakable"))
        {
            if (other.gameObject.TryGetComponent<IHittable>(out var handler))
                handler.OnHit(transform, 0);
            if (_data.DestroyOnImpactWithTarget)
                Destroy(gameObject);
        }
        else if (_sourcePlayer.CompareTag("Breakable") && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(other.gameObject.GetComponent<CharacterController2D>().Hit(_sourcePlayer));
            if (_data.DestroyOnImpactWithTarget)
            {
                Debug.Log(name + " collided with target " + other.name);
                Destroy(gameObject);   
            }
        }

        if (_data.DestroyOnCollisionWithGround && other.gameObject.CompareTag("Ground"))
        {
            Debug.Log(name + " collided with ground " + other.name);
            Destroy(gameObject);
        }
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Breakable"))
            OnTriggerEnter2D(other);
    }
}
