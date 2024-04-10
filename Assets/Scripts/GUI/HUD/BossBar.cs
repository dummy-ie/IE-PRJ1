using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _barRect;

    public void SetHealth(int current, int max)
    {
        // replace with the width of the rect later idk im kinda retarded
        float targetX = Math.Abs((float)current / (float)max - 1) * -225.0f;
        DOTween.To(() => _barRect.anchoredPosition.x, x => _barRect.anchoredPosition = new(x, _barRect.anchoredPosition.y), targetX, 0.5f);
    }
}
