using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HitData hitData = new HitData(1, new Vector2(1, 1));
            collision.gameObject.GetComponent<CharacterController2D>().StartHit(hitData);
        }
    }
}
