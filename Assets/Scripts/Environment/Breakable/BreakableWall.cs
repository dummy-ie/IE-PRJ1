using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IHittable {
    [SerializeField] int _health = 3;
    public void OnHit(int damage) {
        _health--;
        if (_health <= 0)
            Destroy(gameObject);
    }
}
