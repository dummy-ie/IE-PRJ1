using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : Singleton<PauseManager>
{
    private bool _gameIsPaused = false;

    public void OnPauseGame()
    {
        _gameIsPaused = !_gameIsPaused;
        
        if (_gameIsPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        _gameIsPaused = false;
        Time.timeScale = 1.0f;
        HUDManager.Instance.HidePause();
    }

    public void PauseGame()
    {
        _gameIsPaused = true;
        Time.timeScale = 0.0f;
        HUDManager.Instance.ShowPause();
    }

    void OnEnable()
    {
        InputReader.Instance.PauseEvent += OnPauseGame;
    }

    void OnDisable()
    {
        InputReader.Instance.PauseEvent -= OnPauseGame;
    }
}
