using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManiteAdd : MonoBehaviour
{
    CharacterController2D _controller;
    [SerializeField] private int _maniteIncrease = 10;
    private bool pickUp = false;

    public bool PickUp {
        set { pickUp = value; }
    }
    private void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (pickUp) {
            if (_controller.Stats.Manite.Current <= _maniteIncrease)
            {
                _controller.Stats.Manite.Current += _maniteIncrease;
            }
            else
                _controller.Stats.Manite.Current = _controller.Stats.Manite.Max;
            pickUp = false;
        }
    }
}
