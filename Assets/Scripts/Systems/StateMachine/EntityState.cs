using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState<TEntity> : EntityStateMachine<TEntity>, IState where TEntity : EntityStateMachine<TEntity>
{
    public TEntity _entity;
    protected EntityState(TEntity entity)
    {
        _entity = entity;
    }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
