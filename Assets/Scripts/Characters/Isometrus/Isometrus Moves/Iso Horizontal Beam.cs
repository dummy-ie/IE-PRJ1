using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoHorizontalBeam : BaseMove
{
    [SerializeField]
    GameObject _isoBeam;

    [SerializeField] GameObject IsoNodeH1;
    [SerializeField] GameObject IsoNodeH2;

    [SerializeField] GameObject IsoBeamNode1;
    [SerializeField] GameObject IsoBeamNode2;
    [SerializeField] GameObject IsoBeamNode3;
    [SerializeField] GameObject IsoBeamNode4;

    Vector3 _beamDir = Vector3.zero;

    public override void TriggerMove()
    {
        Debug.Log("Horizontal Beam");
        _moveOngoing = true;
        IsoHorizontalBeamMove();
        SelectSequence();
        _isometrus.startTime(4);
    }

    void IsoHorizontalBeamMove()
    {
        if (_moveOngoing)
        {
            switch (Random.Range(0, 2))
            {
                case 0: _isometrus.transform.position = IsoNodeH1.transform.position; _beamDir = Vector3.right; break;
                case 1: _isometrus.transform.position = IsoNodeH2.transform.position; _beamDir = Vector3.left; break;
            }
        }
    }

    void SelectSequence()
    {
        switch (Random.Range(0,3))
        {
            case 0: StartCoroutine(FireSequence1()); break;
            case 1: StartCoroutine(FireSequence2()); break;
            case 2: StartCoroutine(FireSequence3()); break;
        }
    }

    IEnumerator FireSequence1()
    {
        FireBeam(IsoBeamNode1);
        FireBeam(IsoBeamNode2);
        yield return new WaitForSeconds(2f);
        FireBeam(IsoBeamNode3);
        FireBeam(IsoBeamNode4);
    }

    IEnumerator FireSequence2()
    {
        FireBeam(IsoBeamNode1);
        yield return new WaitForSeconds(1f);
        FireBeam(IsoBeamNode2);
        yield return new WaitForSeconds(1f);
        FireBeam(IsoBeamNode3);
        yield return new WaitForSeconds(1f);
        FireBeam(IsoBeamNode4);
    }

    IEnumerator FireSequence3()
    {
        FireBeam(IsoBeamNode3);
        FireBeam(IsoBeamNode2);
        yield return new WaitForSeconds(2f);
        FireBeam(IsoBeamNode1);
        FireBeam(IsoBeamNode4);
    }


    void FireBeam(GameObject IsoBeamNode)
    {
        GameObject projectile = Instantiate(
                _isoBeam, // Beam prefab
                new Vector3(_isometrus.transform.position.x, IsoBeamNode.transform.position.y, _isometrus.transform.position.z), // spawn point
                Quaternion.identity); // rotate (handled by projectile direction)

        var temp = projectile.GetComponentInChildren<LaserProjectile>(); // get in CHILD components
        temp.SourcePlayer = gameObject;

        temp.SetDirection(_beamDir);
    }

}
