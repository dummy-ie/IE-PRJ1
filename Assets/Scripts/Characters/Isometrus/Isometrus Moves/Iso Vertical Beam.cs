using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoVerticalBeam : BaseMove
{
    [SerializeField]
    GameObject _isoBeam;

    [SerializeField]
    GameObject IsoNodeV;

    
    void Update()
    {
        IsoVerticalBeamMove();
    }

    public override void TriggerMove()
    {
        Debug.Log("Vertical Beam");
        _moveOngoing = true;
        FireBeam();
        _isometrus.startTime(2);
    }

    void IsoVerticalBeamMove()
    {
        if (_moveOngoing)
        {
            Vector3 position = new Vector3(_isometrus.controller.transform.position.x, IsoNodeV.transform.position.y, _isometrus.controller.transform.position.z);
            Isometrus.transform.position = position;
            
        }      
    }

    void FireBeam()
    {
        GameObject projectile = Instantiate(
                _isoBeam, // Beam prefab
                _isometrus.transform.position, // spawn point
                Quaternion.identity); // rotate (handled by projectile direction)

        var temp = projectile.GetComponentInChildren<LaserProjectile>(); // get in CHILD components
        temp.SourcePlayer = gameObject;

        temp.SetDirection(Vector3.down); // direction will be normalized to 4 cardinal directions. 
                                         // pass Vector3.up for north, down for south, right for east, left for west.
                                         // (i'm multiplying this one by -1 if the player is to the left but isometrus has set positions anyway so just pass the 4 cardinal directions maybe [or dont])
    }

    private float GetPlayerDirection()
    {
        float value = _isometrus.controller.transform.position.x - transform.position.x;
        if (value < 0)
            return -1;
        else
            return 1;
    }
}
