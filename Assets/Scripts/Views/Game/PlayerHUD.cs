using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : View {
    private ProgressBar _maniteBar;
    public override void Initialize()
    {
        _maniteBar = _root.Q<ProgressBar>("ManiteBar");
    }
}
