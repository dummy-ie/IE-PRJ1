using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundBreakable : MonoBehaviour, IHittable
{
    [SerializeField]
    protected int _health = 1;

    [SerializeField]
    public InteractableData _breakData;

    private GroundPound _groundPound;
    void Start()
    {
    }
    public void OnHit(Transform source, int damage)
    {
        Debug.Log(name + " OnHit from: " + source.name);
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPound>() != null)
        {
            Debug.Log("ground pound not null");
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPound>().IsGroundPound)
            {
                Debug.Log(name + " hit");
                if (_health - 1 >= 0)
                    _health--;

                if (_health <= 0)
                {
                    //_breakData.WasInteracted = true;
                    HitBehavior();
                }
            }
        }
    }

    protected virtual void HitBehavior()
    {
         Debug.Log(name + " killing itself");
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
