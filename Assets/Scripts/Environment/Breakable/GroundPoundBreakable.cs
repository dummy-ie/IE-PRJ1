using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundBreakable : MonoBehaviour, IEntityHittable
{
    [SerializeField]
    protected int _health = 1;

    [SerializeField]
    public InteractableData _breakData;

    private GroundPound _groundPound;
    void Start()
    {
    }
    public void OnHit(HitData hitData)
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPound>() != null)
        {
            Debug.Log("ground pound not null");
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPound>().IsGroundPound)
            {
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
