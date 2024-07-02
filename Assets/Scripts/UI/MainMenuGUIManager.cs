using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using TMPro;

public class MainMenuGUIManager : CanvasMenu
{
    [SerializeField] private CanvasGroup _mainGroup;

    [SerializeField] private SaveSlotsGUIManager _saveSlots;
    [SerializeField] private OptionsGUIManager _options;
    [SerializeField] private ExtrasGUIManager _extras;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _extrasButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private TMP_Text _versionText;

    public IMenuScreen activeScreen;

    private void Awake()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        _extrasButton.onClick.AddListener(OnExtrasButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);

        _saveSlots.OnMenuDisable += ActivateMenu;
        _options.OnMenuDisable += ActivateMenu;
        _extras.OnMenuDisable += ActivateMenu;

        _versionText.text = Application.version;

        InputReader.Instance.MenuCloseEvent += CloseMenu;
        InputReader.Instance.EnableMenuInput();
    }

    private void OnDestroy()
    {
        InputReader.Instance.MenuCloseEvent -= CloseMenu;
    }

    void OnPlayButtonClicked() => SwitchToScreen(_saveSlots);
    void OnOptionsButtonClicked() => SwitchToScreen(_options);
    void OnExtrasButtonClicked() => SwitchToScreen(_extras);
    public void SwitchToScreen(IMenuScreen screen)
    {
        Debug.Log("Switching");
        _mainGroup.FadeGroup(false, UIUtility.TransitionTime, screen.ActivateMenu);
        isEnabled = false;
        activeScreen = screen;
    }
    public void ActivateMenu()
    {
        activeScreen = null;
        isEnabled = true;
        _mainGroup.FadeGroup(true, UIUtility.TransitionTime);
    }
    private void CloseMenu()
    {
        activeScreen?.DeactivateMenu();
    }
    void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
