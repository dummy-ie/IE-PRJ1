using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private string _nextScene;

    VisualElement _root;
    Button _playButton;
    Button _settingsButton;
    Button _quitButton;
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _playButton = _root.Q<Button>("PlayButton");
        _settingsButton = _root.Q<Button>("SettingsButton");
        _quitButton = _root.Q<Button>("QuitButton");

        _playButton.clicked += OnPlayButtonPressed;
        _settingsButton.clicked += OnSettingsButtonPressed;
        _quitButton.clicked += OnQuitButtonPressed;
    }

    void OnPlayButtonPressed() {
        SceneLoader.Instance.LoadScene(_nextScene ,true);
    }
    void OnSettingsButtonPressed() { }
    void OnQuitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
