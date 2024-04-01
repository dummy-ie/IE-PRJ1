using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected IState currentState;
    protected virtual void SwitchState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
