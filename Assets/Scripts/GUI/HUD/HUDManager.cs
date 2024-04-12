using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField]
    PlayerStatField _playerStats;

    [SerializeField]
    private RectTransform _maniteBarRect;

    [SerializeField]
    private RectTransform _healthBarRect;

    [SerializeField] private PauseScreenUI _pauseScreen;
    // [SerializeField]
    // private RectMask2D _mask;

    // private float _maxRightMask;
    // private float _initialRightMask;

    // [SerializeField]
    // GameObject _heart;
    // [SerializeField]
    // GameObject _emptyHeart;
    // [SerializeField]
    // RectTransform _heartContainer;

    [SerializeField] private GameObject _healthText;
    [SerializeField] private GameObject _maniteText;

    public void ShowPause()
    {
        _pauseScreen.gameObject.SetActive(true);
    }

    public void HidePause()
    {
        _pauseScreen.gameObject.SetActive(false);
    }

    public void SetManiteValue(int value)
    {
        // float targetWidth = value * _maxRightMask / _playerStats.Manite.Max;
        // float newRightMask = _maxRightMask + _initialRightMask - targetWidth;
        // Vector4 padding = _mask.padding;
        // padding.z = newRightMask;
        // DOTween.To(()=> _mask.padding, x=> _mask.padding = x, padding, 0.5f);
        float targetX = Math.Abs((float)value / _playerStats.Manite.Max - 1) * -_maniteBarRect.rect.width;
        DOTween.To(
            () => _maniteBarRect.anchoredPosition.x,
            x => _maniteBarRect.anchoredPosition = new Vector2(x, _maniteBarRect.anchoredPosition.y),
            targetX,
            0.5f
        );
        // Debug.Log("manite bar " + targetX);
        _maniteText.GetComponent<TextMeshProUGUI>().text = value + " / " + _playerStats.Manite.Max;
    }

    public void SetHearts(int value)
    {
        // float padding = (_heart.GetComponent<Image>().preferredWidth + _emptyHeart.GetComponent<Image>().preferredWidth) / 2;
        // int i = 0;
        // for (; i < value; i++) {
        //     GameObject clone = Instantiate(_heart, _heartContainer);
        //     RectTransform rectTransform = clone.GetComponent<RectTransform>();
        //     rectTransform.localPosition = new Vector3(i * padding, 0);
        // }
        // for (; i < _playerStats.Health.Max; i++) {
        //     GameObject clone = Instantiate(_emptyHeart, _heartContainer);
        //     RectTransform rectTransform = clone.GetComponent<RectTransform>();
        //     rectTransform.localPosition = new Vector3(i * padding, 0);
        // }
        if (_healthBarRect != null)
        {
            float targetX = Math.Abs((float)value / (float)_playerStats.Health.Max - 1) * -_healthBarRect.rect.width;
            DOTween.To(
                () => _healthBarRect.anchoredPosition.x,
                x => _healthBarRect.anchoredPosition = new Vector2(x, _healthBarRect.anchoredPosition.y),
                targetX,
                0.5f
            );
            // Debug.Log("health bar width " + _healthBarRect.rect.width);
            // Debug.Log("player stat hp max " + _playerStats.Health.Max);
            // Debug.Log("health bar " + targetX);
            // Debug.Log("health bar anchored pos " + _healthBarRect.anchoredPosition);
            _healthText.GetComponent<TextMeshProUGUI>().text = value + " / " + _playerStats.Health.Max;
        }
        
    }

    private void OnEnable()
    {
        // Debug.Log("HUDManager");
        /*_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().Stats;
        _playerStats.Health.CurrentChanged += SetHearts;
        _playerStats.Manite.CurrentChanged += SetManiteValue;
        SetManiteValue(_playerStats.Manite.Current);
        SetHearts(_playerStats.Health.Current);*/
        _maniteBarRect.anchoredPosition = Vector2.zero;
        _healthBarRect.anchoredPosition = Vector2.zero;
    }

    private void OnDisable()
    {
        //_playerStats.Health.CurrentChanged -= SetHearts;
        //_playerStats.Manite.CurrentChanged -= SetManiteValue;
    }

    void Update()
    {
        //SetManiteValue(_playerStats.Manite.Current);
        //SetHearts(_playerStats.Health.Current);
    }

    private void Start()
    {
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().Stats;
        // _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        // _initialRightMask = _mask.padding.z;
        // Debug.Log("HUDManagerStart");
        // Debug.Log($"Player Hearts : {_playerStats.Health.Current}");
        //_playerStats.Health.CurrentChanged += SetHearts;
        //_playerStats.Manite.CurrentChanged += SetManiteValue;
        _maniteBarRect.anchoredPosition = Vector2.zero;
        _healthBarRect.anchoredPosition = Vector2.zero;
        SetManiteValue(_playerStats.Manite.Current);
        SetHearts(_playerStats.Health.Current);
    }
}
