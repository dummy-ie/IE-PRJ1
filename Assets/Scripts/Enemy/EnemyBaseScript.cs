using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBaseScript : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected bool isFacingRight;

    [SerializeField]
    EnemyData enemyData;

    int currentHealth;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float boxCastDistance = 0.4f;
    [SerializeField] Vector2 boxSize = new(0.3f, 0.4f);
    [SerializeField] LayerMask groundLayer;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = enemyData.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentHealth <= 0)
        {
            this.gameObject.SetActive(false); //OR Destroy(this.gameObject);
        }
    }

    virtual protected void Flip()
    {
        if (isFacingRight && rb.velocity.x < 0f || !isFacingRight && rb.velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    virtual public void Hit(GameObject player, Vector2 dmgSourcePos, int damageTaken = 0)
    {
        Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, transform.position.y - dmgSourcePos.y);
        vec.Normalize();
        rb.AddForce(vec * 5, ForceMode2D.Impulse);
        Debug.Log("Hit");
    }

    virtual public void Damage(int amount)
    {

        if (this.currentHealth - amount >= 0)
        {
            this.currentHealth -= amount;
        }

        else
        {
            this.currentHealth = 0;
        }

    }

    protected bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, boxCastDistance, groundLayer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(collision.gameObject.GetComponent<CharacterController2D>().Hit(gameObject, enemyData.damage));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (boxCastDistance * -transform.up), boxSize);
    }
}
