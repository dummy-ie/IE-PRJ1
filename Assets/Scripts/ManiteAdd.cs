using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManiteAdd : MonoBehaviour
{
    CharacterController2D controller;
    [SerializeField] private int maniteIncrease = 10;
    private bool pickUp = false;

    public bool PickUp {
        set { pickUp = value; }
    }
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (pickUp) {
            controller.AddManite(maniteIncrease);
            pickUp = false;
        }
    }
}
