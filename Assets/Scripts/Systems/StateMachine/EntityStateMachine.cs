using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateMachine<TEntity> : StateMachine where TEntity : EntityStateMachine<TEntity>
{
}
