using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    PlayerStatField _playerStats;

    private void Start()
    {
        _playerStats.Health.SetMax(3);
        _playerStats.Manite.SetMax(100);
        _playerStats.Health.SetCurrent(3);
        _playerStats.Manite.SetCurrent(100);
    }
}
