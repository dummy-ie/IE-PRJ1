using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    
}*/

public abstract class Singleton : MonoBehaviour
{
    [CanBeNull]
    private static Singleton _instance;
    public static Singleton Instance {
        get { return _instance; }
    }

    [SerializeField]
    bool _persistent = true;

    protected virtual void OnAwake() { }
    void Awake() {
        if (_instance != null) { 
            _instance = this;
        }
        else
            Destroy(gameObject);
        if (_persistent)
            DontDestroyOnLoad(gameObject);
        OnAwake();
    }
}
