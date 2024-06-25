using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryResultsScript : MonoBehaviour
{
    public void OnMenuButtonClicked()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
