using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalProjectile : MonoBehaviour
{
    private float lifespanCounter = 0f;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float lifespanTime = 1f;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float speed = 1f;

    [SerializeField]
    private bool destroyOnImpactWithTarget = true;

    [SerializeField]
    private bool destroyOnCollisionWithGround = true;

    GameObject sourcePlayer;
    public GameObject SourcePlayer
    {
        get { return sourcePlayer; }
        set { sourcePlayer = value; }
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
        float projectileSpeed = speed * CheckFlipped() * Time.deltaTime;
        // Debug.Log("slash projectile sped: " + projectileSpeed);
        if (lifespanCounter <= lifespanTime)
            transform.Translate(new Vector3(projectileSpeed, 0, 0), Space.World);
    }

    private void Update()
    {
        lifespanCounter += Time.deltaTime;

        if (lifespanCounter > lifespanTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (sourcePlayer.CompareTag("Player") && other.gameObject.CompareTag("Breakable"))
        {
            other.gameObject.GetComponent<EnemyBaseScript>().Hit(sourcePlayer, transform.position);
            if (destroyOnImpactWithTarget)
                Destroy(gameObject);
        }
        else if (sourcePlayer.CompareTag("Breakable") && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(other.gameObject.GetComponent<CharacterController2D>().Hit(sourcePlayer));
            if (destroyOnImpactWithTarget)
                Destroy(gameObject);
        }

        if (destroyOnCollisionWithGround && other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
