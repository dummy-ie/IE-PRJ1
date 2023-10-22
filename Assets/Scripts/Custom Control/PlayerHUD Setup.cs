using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUISetup : MonoBehaviour
{
    private void OnEnable() { 
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        PlayerHUD playerUI = new PlayerHUD();
    }
}
