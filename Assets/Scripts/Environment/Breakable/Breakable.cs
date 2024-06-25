using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IEntityHittable
{
    [SerializeField]
    protected int _health = 3;

    [SerializeField]
    public InteractableData _breakData;

    public void OnHit(HitData hitData)
    {
        // Debug.Log(name + " OnHit from: " + source.name);
        if (_health - 1 >= 0)
            _health--;

        if (_health <= 0)
        {
            _breakData.Enabled = true;
            HitBehavior();
        }
    }

    protected virtual void HitBehavior()
    {
         Debug.Log(name + " killing itself");
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
