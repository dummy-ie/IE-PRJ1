using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoRangedAttack : BaseMove
{
    [SerializeField]
    GameObject IsoNodeR1;
    [SerializeField]
    GameObject IsoNodeR2;
    [SerializeField]
    GameObject IsoNodeR3;
    [SerializeField]
    GameObject IsoNodeR4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerMove()
    {
        Debug.Log("Ranged Attack");
        _moveOngoing = true;
        _isometrus.startTime();
        IsoRangedAttackMove();
    }

    void IsoRangedAttackMove()
    {
        if (_moveOngoing)
        {
            switch (Random.Range(0, 4))
            {
                case 0: _isometrus.transform.position = IsoNodeR1.transform.position; break;
                case 1: _isometrus.transform.position = IsoNodeR2.transform.position; break;
                case 2: _isometrus.transform.position = IsoNodeR3.transform.position; break;
                case 3: _isometrus.transform.position = IsoNodeR4.transform.position; break;
            }
        }
    }
}
