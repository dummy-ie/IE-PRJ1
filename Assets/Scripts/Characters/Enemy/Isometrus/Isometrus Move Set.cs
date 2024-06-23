using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometrusMoveSet : MonoBehaviour
{
    [SerializeField]
    public CharacterController2D controller;

    bool _moveOngoing = false; 
    BaseMove _currentMove;

    IsoHorizontalBeam _hBeam;
    IsoVerticalBeam _vBeam;
    IsoRangedAttack _rAttack;

    IsoMoves _isoMoves = IsoMoves.RATTACK;

    enum IsoMoves
    {
        HBEAM = 0,
        VBEAM,
        RATTACK
    }

    public void CycleIsometrusMoves()
    {
        if (!_moveOngoing)
        {
            switch (_isoMoves)
            {
                case IsoMoves.HBEAM: _currentMove = _hBeam; _isoMoves = IsoMoves.VBEAM; break;
                case IsoMoves.VBEAM: _currentMove = _vBeam; _isoMoves = IsoMoves.RATTACK; break;
                case IsoMoves.RATTACK: _currentMove = _rAttack; _isoMoves = IsoMoves.HBEAM; break;
            }


            _currentMove.TriggerMove();
        }


        _moveOngoing = _currentMove.MoveOngoing;
    }

    public void startTime(float time)//Degub
    {
        StartCoroutine(_currentMove.Cooldown(time));
    }




    private void Awake()
    {
        _hBeam = GetComponent<IsoHorizontalBeam>();
        _vBeam = GetComponent<IsoVerticalBeam>();
        _rAttack = GetComponent<IsoRangedAttack>();


        _hBeam.Isometrus = this;
        _vBeam.Isometrus = this;
        _rAttack.Isometrus = this;
    }

    private void Update()
    {
        CycleIsometrusMoves();
    }
}

