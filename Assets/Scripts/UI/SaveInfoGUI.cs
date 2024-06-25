using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveInfoGUI : CanvasMenu, IMenuScreen
{

    [SerializeField] private CanvasGroup _canvasGroup;
    public event System.Action OnMenuDisable;

    //ALL TEMPORARY
    [SerializeField] private TMP_Text _healthText;
    public string HealthText
    {
        get { return _healthText.text; }
        set { _healthText.text = value; }
    }

    [SerializeField] private TMP_Text _maniteText;
    public string ManiteText
    {
        get { return _maniteText.text; }
        set { _maniteText.text = value; }
    }

    [SerializeField] private TMP_Text _biomeText;
    public string BiomeText
    {
        get { return _biomeText.text; }
        set { _biomeText.text = value; }
    }

    [SerializeField] private TMP_Text _timePlayedText;
    public string TimePlayedText
    {
        get { return _timePlayedText.text; }
        set { _timePlayedText.text = value; }
    }

    [SerializeField] private TMP_Text _lastPlayedText;
    public string LastPlayedText
    {
        get { return _lastPlayedText.text; }
        set { _lastPlayedText.text = value; }
    }

    public void ActivateMenu()
    {
        isEnabled = true;
        _canvasGroup.FadeGroup(true, UIUtility.TransitionTime);
    }

    public void DeactivateMenu()
    {
        isEnabled = false;
        _canvasGroup.FadeGroup(false, UIUtility.TransitionTime, () => OnMenuDisable?.Invoke());
    }
}
