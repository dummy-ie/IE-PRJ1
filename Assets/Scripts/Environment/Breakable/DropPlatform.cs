using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : Breakable
{
    [SerializeField]
    private List<GameObject> _tethers;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    protected override void HitBehavior()
    {
        // bool noTethers = true;
        // foreach (GameObject tether in _tethers)
        // {
        //     if (tether)
        //     {
        //         // Breakable a = tether.GetComponent<Breakable>();
        //         // Debug.Log("Tether: " + tether.name + " " + tether.activeInHierarchy + " " + tether.activeSelf + " " + a._breakData.WasInteracted);
        //         noTethers = false;
        //         break;
        //     }
        // }
            
        _tethers.RemoveAll(x => !x || x == null);

        if (_tethers.Count == 0)
        {
            _rb.gravityScale = 1;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Drop platform triggered: " + other.name);
        if (other.name == "Stopper")
        {
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}