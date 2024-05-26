using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasGUIManager : CanvasMenu, IMenuScreen
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public event System.Action OnMenuDisable;

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
