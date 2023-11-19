using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoVerticalBeam : BaseMove
{

    [SerializeField]
    GameObject IsoNodeV;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        IsoVerticalBeamMove();
    }

    public override void TriggerMove()
    {
        Debug.Log("Vertical Beam");
        _moveOngoing = true;
        _isometrus.startTime();
    }

    void IsoVerticalBeamMove()
    {
        if (_moveOngoing)
        {
            Vector3 position = new Vector3(_isometrus.controller.transform.position.x, IsoNodeV.transform.position.y, _isometrus.controller.transform.position.z);
            Isometrus.transform.position = position;
        }
    }
}
