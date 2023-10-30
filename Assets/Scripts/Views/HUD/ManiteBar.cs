using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManiteBar : View {
    CharacterController2D controller;
    ProgressBar maniteBar;
    public override void Initialize() {
        this.controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>(); 
        this.maniteBar = this._root.Q<ProgressBar>("ManiteBar");
    }

    void Update()
    {
        this.maniteBar.highValue = controller.MaxManite;
        this.maniteBar.value = controller.CurrentManite;
        this.maniteBar.title = this.maniteBar.value + "/" + this.maniteBar.highValue;
    }
}
