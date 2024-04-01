using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class View : MonoBehaviour {
    [SerializeField]
    bool _onStart = false;

    private int _sortingOrder = 0;
    public bool OnStart { 
        get { return _onStart; } 
    }
    public abstract void Initialize();

    public virtual void Hide() {
        this.gameObject.SetActive(false);
    }

    public virtual void Show() {
        this.gameObject.SetActive(true);
    }

    protected void OnEnable() {
        this.Initialize();
    }
}
