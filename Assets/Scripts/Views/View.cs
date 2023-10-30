using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class View : MonoBehaviour {
    [SerializeField]
    bool _onStart = false;
    public bool OnStart { 
        get { return _onStart; } 
    }
    //[SerializeField]
    protected UIDocument _document;
    public UIDocument Document { 
        get { return _document; }
    }
    protected VisualElement _root;
    public abstract void Initialize();

    public virtual void Hide() {
        //this.gameObject.SetActive(false);
        this._root.style.display = DisplayStyle.None;
    }

    public virtual void Show() {
        // this.gameObject.SetActive(true);
        this._root.style.display = DisplayStyle.Flex;
    }

    protected void OnEnable() {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;
        this.Initialize();
    }
}
