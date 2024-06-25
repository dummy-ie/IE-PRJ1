using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTrunk : Breakable
{
    [SerializeField]
    GameObject _vine;
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.StopPlayback();
    }

    // Update is called once per frame
    void Update()
    {

        if (_vine == null)
        {
            _anim.Play("BridgeFall");
        }
    }

    protected override void HitBehavior()
    {
    }
}
