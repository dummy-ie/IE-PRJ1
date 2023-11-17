using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IHittable {
    [SerializeField] int _health = 3;
    [SerializeField] InteractableData _breakData;
    public void OnHit(Transform source, int damage) {
        _health--;
        if (_health <= 0){
            _breakData._wasInteracted = true;
            Destroy(gameObject);    
        }
            
    }

}
