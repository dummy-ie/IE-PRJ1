using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField]
    PlayerStatField _playerStats;

    [SerializeField]
    private RectTransform _barRect;

    [SerializeField]
    private RectMask2D _mask;

    private float _maxRightMask;
    private float _initialRightMask;

    [SerializeField]
    GameObject _heart;
    [SerializeField]
    GameObject _emptyHeart;
    [SerializeField]
    RectTransform _heartContainer;

    public void SetManiteValue(int value) {
        float targetWidth = value * _maxRightMask / _playerStats.Manite.Max;
        float newRightMask = _maxRightMask + _initialRightMask - targetWidth;
        Vector4 padding = _mask.padding;
        padding.z = newRightMask;
        DOTween.To(()=> _mask.padding, x=> _mask.padding = x, padding, 0.5f);
    }

    public void SetHearts(int value) {
        float padding = (_heart.GetComponent<Image>().preferredWidth + _emptyHeart.GetComponent<Image>().preferredWidth) / 2;
        int i = 0;
        for (; i < value; i++) {
            GameObject clone = Instantiate(_heart, _heartContainer);
            Debug.Log(i);
            RectTransform rectTransform = clone.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(i * padding, 0);
        }
        for (; i < _playerStats.Health.Max; i++) {
            GameObject clone = Instantiate(_emptyHeart, _heartContainer);
            Debug.Log(i);
            RectTransform rectTransform = clone.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(i * padding, 0);
        }
    }

    private void OnEnable()
    {
        Debug.Log("HUDManager");
        _playerStats.Health.CurrentChanged += SetHearts;
        _playerStats.Manite.CurrentChanged += SetManiteValue;
    }

    private void OnDisable()
    {
        _playerStats.Health.CurrentChanged -= SetHearts;
        _playerStats.Manite.CurrentChanged -= SetManiteValue;
    }

    private void Start()
    {
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _initialRightMask = _mask.padding.z;
        Debug.Log("HUDManagerStart");
        Debug.Log($"Player Hearts : {_playerStats.Health.Current}");
        SetManiteValue(_playerStats.Manite.Current);
        SetHearts(_playerStats.Health.Current);
    }
}
