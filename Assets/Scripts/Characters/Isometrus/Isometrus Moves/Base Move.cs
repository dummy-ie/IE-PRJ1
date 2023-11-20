using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    protected IsometrusMoveSet _isometrus;
    public IsometrusMoveSet Isometrus
    {
        get { return _isometrus; }
        set { _isometrus = value; }
    }

    protected bool _moveOngoing = false;
    public bool MoveOngoing
    {
        get { return _moveOngoing; }
        set { _moveOngoing = value; }
    }

    public virtual void TriggerMove()
    {

    }

    public IEnumerator Cooldown(float time) // testing
    {
        yield return new WaitForSeconds(time);
        _moveOngoing = false;
    }

}
