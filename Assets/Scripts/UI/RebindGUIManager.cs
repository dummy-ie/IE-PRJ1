using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class RebindGUIManager : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }
    void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

}
