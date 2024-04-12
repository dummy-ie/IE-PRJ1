using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : Singleton<PauseManager>
{
    private bool _gameIsPaused = false;

    public void OnPauseGame(InputAction.CallbackContext context)
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
        HUDManager.Instance.HidePause();
        _gameIsPaused = false;
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        HUDManager.Instance.ShowPause();
        _gameIsPaused = true;
        Time.timeScale = 0.0f;
    }
}
