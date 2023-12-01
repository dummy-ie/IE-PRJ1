using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBaseScript : MonoBehaviour, IHittable
{
    protected Rigidbody2D _rb;

    protected bool _isFacingRight;

    [SerializeField]
    EnemyData _enemyData;

    [SerializeField]
    GameObject _manitePrefab;

    protected int _currentHealth;

    private Material _mat;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);
    [SerializeField] LayerMask _groundLayer;

    virtual public void OnHit(Transform source, int damage)
    {
        Vector2 vec = new Vector2(transform.position.x - source.position.x, transform.position.y - source.position.y);
        vec.Normalize();
        _rb.AddForce(vec * 5, ForceMode2D.Impulse);
        StartCoroutine(Blink());
        Debug.Log("Hit");
    }
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _enemyData.health;
        _mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._currentHealth <= 0)
        {
            Debug.Log("Enemy Killed");
            Instantiate(_manitePrefab, transform.position, transform.rotation);
            this.gameObject.SetActive(false); //OR Destroy(this.gameObject);
        }
    }

    virtual protected void Flip()
    {
        if (_isFacingRight && _rb.velocity.x < 0f || !_isFacingRight && _rb.velocity.x > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /*virtual public void Hit(GameObject player, Vector2 dmgSourcePos, int damageTaken = 0)
    {
        Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, transform.position.y - dmgSourcePos.y);
        vec.Normalize();
        _rb.AddForce(vec * 5, ForceMode2D.Impulse);
        Debug.Log("Hit");
    }*/

    virtual public void Damage(int amount)
    {

        if (this._currentHealth - amount >= 0)
        {
            this._currentHealth -= amount;
        }

        else
        {
            this._currentHealth = 0;
        }

    }

    protected bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _groundLayer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(collision.gameObject.GetComponent<CharacterController2D>().Hit(gameObject, _enemyData.damage));
        }
    }

    protected IEnumerator Blink()
    {
        SetColor(new Color(0.5f, 0.5f, 0.5f, 1));
        yield return new WaitForSeconds(0.5f);
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        _mat.SetColor("_Color", color);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (_boxCastDistance * -transform.up), _boxSize);
    }
}
