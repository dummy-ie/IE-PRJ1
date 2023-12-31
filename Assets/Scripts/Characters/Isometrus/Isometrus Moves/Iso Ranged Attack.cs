using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoRangedAttack : BaseMove
{
    [SerializeField]
    GameObject _isoProjectile;

    [SerializeField]GameObject IsoNodeR1;
    [SerializeField]GameObject IsoNodeR2;
    [SerializeField]GameObject IsoNodeR3;
    [SerializeField]GameObject IsoNodeR4;
    [SerializeField]GameObject IsoNodeR5;
    [SerializeField]GameObject IsoNodeR6;



    [SerializeField]
    int _initialProjectileAmt = 5;


    public override void TriggerMove()
    {
        Debug.Log("Ranged Attack");
        _moveOngoing = true;
        IsoRangedAttackMove();
        StartCoroutine(FireProjectile());
        _isometrus.startTime(3);
        
    }

    void IsoRangedAttackMove()
    {
        if (_moveOngoing)
        {
            switch (Random.Range(0, 6))
            {
                case 0: _isometrus.transform.position = IsoNodeR1.transform.position; break;
                case 1: _isometrus.transform.position = IsoNodeR2.transform.position; break;
                case 2: _isometrus.transform.position = IsoNodeR3.transform.position; break;
                case 3: _isometrus.transform.position = IsoNodeR4.transform.position; break;
                case 4: _isometrus.transform.position = IsoNodeR5.transform.position; break;
                case 5: _isometrus.transform.position = IsoNodeR6.transform.position; break;
            }
        }      
    }

    IEnumerator FireProjectile()
    {
        GameObject projectile = Instantiate(
                _isoProjectile, // projectile prefab
                new Vector3(transform.position.x + 1f * GetPlayerDirection(), transform.position.y, transform.position.z), // spawn position
                Quaternion.identity); // spawn rotation (is handled by DirectionalProjectile)

        // set source and target
        var temp = projectile.GetComponent<DirectionalProjectile>();
        temp.SourcePlayer = gameObject;

        temp.SetTarget(_isometrus.controller.transform);

        yield return new WaitForSeconds(.3f);

        if (_moveOngoing)
        {
            StartCoroutine(FireProjectile());
        }
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
