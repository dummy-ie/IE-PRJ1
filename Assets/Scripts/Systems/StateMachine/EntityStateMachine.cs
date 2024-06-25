using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateMachine<TEntity> : MonoBehaviour where TEntity : EntityStateMachine<TEntity>
{
    protected IState currentState;
    protected virtual void SwitchState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
