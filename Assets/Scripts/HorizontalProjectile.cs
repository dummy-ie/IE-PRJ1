using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalProjectile : MonoBehaviour
{
    private float _lifespanCounter = 0f;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _lifespanTime = 1f;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float _speed = 1f;

    [SerializeField]
    private bool _destroyOnImpactWithTarget = true;

    [SerializeField]
    private bool _destroyOnCollisionWithGround = true;

    GameObject _sourcePlayer;
    public GameObject SourcePlayer
    {
        get { return _sourcePlayer; }
        set { _sourcePlayer = value; }
    }

    private int CheckFlipped()
    {
        if (transform.localScale.y < 0)
            return 1;

        return -1;
    }

    private void FixedUpdate()
    {
        // Debug.Log("slash projectile flip: " + CheckFlipped());
        float projectileSpeed = _speed * CheckFlipped() * Time.deltaTime;
        // Debug.Log("slash projectile sped: " + projectileSpeed);
        if (_lifespanCounter <= _lifespanTime)
            transform.Translate(new Vector3(projectileSpeed, 0, 0), Space.World);
    }

    private void Update()
    {
        _lifespanCounter += Time.deltaTime;

        if (_lifespanCounter > _lifespanTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_sourcePlayer.CompareTag("Player") && other.gameObject.CompareTag("Breakable"))
        {
            //other.gameObject.GetComponent<EnemyBaseScript>().Hit(_sourcePlayer, transform.position);
            IHittable handler = other.gameObject.GetComponent<IHittable>();
            if (handler != null)
                handler.OnHit(transform, 0);
            if (_destroyOnImpactWithTarget)
                Destroy(gameObject);
        }
        else if (_sourcePlayer.CompareTag("Breakable") && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(other.gameObject.GetComponent<CharacterController2D>().Hit(_sourcePlayer));
            if (_destroyOnImpactWithTarget)
                Destroy(gameObject);
        }

        if (_destroyOnCollisionWithGround && other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
