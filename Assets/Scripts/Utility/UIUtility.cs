using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUtility
{
    public const float TransitionTime = .3f;
    public static Tweener FadeGroup(this CanvasGroup group, bool active, float duration, TweenCallback onComplete = null, bool disableUIEvents = true)
    {
        return DOVirtual.Float(group.alpha, active ? 1 : 0, duration, (a) => group.alpha = a).OnComplete(() => {
            group.blocksRaycasts = active;
            group.interactable = active;
            onComplete?.Invoke();
        }).SetUpdate(true);
    }
}
