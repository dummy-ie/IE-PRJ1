using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiteSlashProjectile : MonoBehaviour
{
    private float lifespanCounter = 0f;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float lifespanTime = 1f;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float speed = 1f;

    [SerializeField]
    GameObject sourcePlayer;

    private int CheckFlipped()
    {
        if (transform.localScale.y < 0)
            return 1;

        return -1;
    }

    private void FixedUpdate()
    {
        Debug.Log("slash projectile flip: " + CheckFlipped());
        float projectileSpeed = speed * CheckFlipped() * Time.deltaTime;
        Debug.Log("slash projectile sped: " + projectileSpeed);
        if (lifespanCounter <= lifespanTime)
            transform.Translate(new Vector3(0,
                                            projectileSpeed,
                                            0));        
    }

    private void Update()
    {
        lifespanCounter += Time.deltaTime;

        if (lifespanCounter > lifespanTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Breakable"))
            other.gameObject.GetComponent<EnemyBaseScript>().Hit(sourcePlayer, transform.position);
    }
}
