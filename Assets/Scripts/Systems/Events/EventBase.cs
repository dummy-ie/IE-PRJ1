using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBase : ScriptableObject
{
    [TextArea()] public string eventDescription;
}

public class EventBase<T0> : EventBase
{
    public UnityEvent<T0> OnEventRaise;
    public void Raise(T0 arg0)
    {
        OnEventRaise?.Invoke(arg0);
    }
}

public class EventBase<T0, T1> : EventBase
{
    public UnityEvent<T0, T1> OnEventRaise;
    public void Raise(T0 arg0, T1 arg1)
    {
        OnEventRaise?.Invoke(arg0, arg1);
    }
}