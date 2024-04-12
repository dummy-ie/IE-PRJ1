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
        float targetX = Math.Abs((float)value / _playerStats.Manite.Max - 1) * -_maniteBarRect.rect.width;
        DOTween.To(
            () => _maniteBarRect.localPosition.x,
            x => _maniteBarRect.localPosition = new Vector3(x, _maniteBarRect.localPosition.y, _maniteBarRect.localPosition.z),
            targetX,
            0.5f
        );
        _maniteText.GetComponent<TextMeshProUGUI>().text = value + " / " + _playerStats.Manite.Max;
    }

    public void SetHearts(int value)
    {
        if (_healthBarRect != null)
        {
            float targetX = Math.Abs((float)value / (float)_playerStats.Health.Max - 1) * -_healthBarRect.rect.width;
            DOTween.To(
                () => _healthBarRect.localPosition.x,
                x => _healthBarRect.localPosition = new Vector3(x, _healthBarRect.localPosition.y, _healthBarRect.localPosition.z),
                targetX,
                0.5f
            );
            _healthText.GetComponent<TextMeshProUGUI>().text = value + " / " + _playerStats.Health.Max;
        }

    }

    private void Start()
    {
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().Stats;

        _healthBarRect.localPosition = new Vector2(0, 0);
        _maniteBarRect.localPosition = new Vector2(0, 0);
        if (_playerStats != null)
        {
            SetManiteValue(_playerStats.Manite.Current);
            SetHearts(_playerStats.Health.Current);
        }
    }
}
