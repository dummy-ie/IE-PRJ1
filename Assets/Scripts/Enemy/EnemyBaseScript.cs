using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(Vector2 dmgSourcePos)
    {
        Vector2 vec = new Vector2(transform.position.x - dmgSourcePos.x, transform.position.y - dmgSourcePos.y);
        vec.Normalize();
        rb.AddForce(vec * 5, ForceMode2D.Impulse);
        Debug.Log("Hit");
    }
}
