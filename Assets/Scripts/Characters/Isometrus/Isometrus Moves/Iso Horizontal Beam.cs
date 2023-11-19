using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoHorizontalBeam : BaseMove
{
    [SerializeField]
    GameObject IsoNodeH1;
    [SerializeField]
    GameObject IsoNodeH2;

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
        Debug.Log("Horizontal Beam");
        _moveOngoing = true;
        _isometrus.startTime();

        IsoHorizontalBeamMove();
    }

    void IsoHorizontalBeamMove()
    {
        if (_moveOngoing)
        {
            switch (Random.Range(0, 2))
            {
                case 0: _isometrus.transform.position = IsoNodeH1.transform.position; break;
                case 1: _isometrus.transform.position = IsoNodeH2.transform.position; break;
            }
        }
    }
}
