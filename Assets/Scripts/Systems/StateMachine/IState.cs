using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public interface IState
{
    public void Enter() {}
    public void Execute() {}
    public void Exit() {}
}
