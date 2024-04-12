using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    public void OnResumeButtonClicked()
    {
        PauseManager.Instance.ResumeGame();
    }

    public void OnSettingsButtonClicked()
    {

    }

    public void OnQuitButtonClicked()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
