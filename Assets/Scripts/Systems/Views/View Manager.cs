using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ViewManager : Singleton<ViewManager> {
    //[SerializeField]
    //private View _startingView;

    //[SerializeField]
    private View[] _views;

    private Stack<View> _currentViews;

    public T GetView<T>() where T : View { 
        for (int i = 0; i < this._views.Length; i++) { 
            if (this._views[i] is T view) {
                return view;
            }
        }
        return default;
    }
    
    public void Show<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                if (this._currentViews.Count != 0)
                    this._currentViews.Pop().Hide();
                view.Show();
                this._currentViews.Push(view);
            }
        }
    }

    public void Show(View view) {
        if (this._currentViews.Count != 0)
            this._currentViews.Pop().Hide();
        view.Show();
        this._currentViews.Push(view);
    }
    
    public void PopUp<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                //if (this._currentViews.Count != 0)
                //    view.Document.sortingOrder = _currentViews.Peek().Document.sortingOrder + 1;
                view.Show();
                this._currentViews.Push(view);
            }
        }
    }

    public void PopUp(View view) {
        //if (this._currentViews.Count != 0)
        //    view.Document.sortingOrder = _currentViews.Peek().Document.sortingOrder + 1;
        view.Show();
        this._currentViews.Push(view);
    }

    public void HideRecentView() {
        this._currentViews.Pop().Hide();
    }

    protected override void OnAwake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _views = FindObjectsOfType<View>();
        for (int i = 0; i < this._views.Length;i++) {
            _views[i].Initialize();
            _views[i].Hide();
            if (_views[i].OnStart) {
                _views[i].Show();
            }
        }
    }
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _views = FindObjectsOfType<View>();
        for (int i = 0; i < this._views.Length;i++) {
            _views[i].Initialize();
            _views[i].Hide();
            if (_views[i].OnStart) {
                _views[i].Show();
            }
        }
    }
}
