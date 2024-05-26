using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SaveSlotsGUIManager : CanvasMenu, IMenuScreen
{
    [SerializeField]
    AssetReference _gameSceneReference;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Button _slot1Button;
    [SerializeField] private Button _slot2Button;
    [SerializeField] private Button _slot3Button;

    public event System.Action OnMenuDisable;

    private bool isMakingNewGame = false;
    // Start is called before the first frame update
    void Start()
    {
        _slot1Button.onClick.AddListener(OnSlot1ButtonClicked);
        _slot2Button.onClick.AddListener(OnSlot2ButtonClicked);
        _slot3Button.onClick.AddListener(OnSlot3ButtonClicked);
    }

    void OnSlot1ButtonClicked()
    {
        Debug.Log("Slot 1 Selected");
        InitializeGame(1);
    }

    void OnSlot2ButtonClicked()
    {
        Debug.Log("Slot 2 Selected");
        InitializeGame(2);
    }

    void OnSlot3ButtonClicked()
    {
        Debug.Log("Slot 3 Selected");
        InitializeGame(3);
    }

    void InitializeGame(int slotNum)
    {
        if (isMakingNewGame)
        {
            DataManager.Instance.SetSelectedSave(slotNum);
            DataManager.Instance.NewRepository();
            SceneLoader.Instance.LoadSceneWithFade(_gameSceneReference, new SceneLoader.TransitionData { spawnPoint = "default" });
        }
        else
        {
            DataManager.Instance.SetSelectedSave(slotNum);
            DataManager.Instance.LoadRepository();
            SceneLoader.Instance.LoadSceneWithFade(new AssetReference(DataManager.Instance.Repository.SavedScene), new SceneLoader.TransitionData { spawnPoint = "default" });
        }
    }

    public void ActivateMenu()
    {
        isEnabled = true;
        _canvasGroup.FadeGroup(true, UIUtility.TransitionTime);
    }

    public void DeactivateMenu()
    {
        isEnabled = false;
        _canvasGroup.FadeGroup(false, UIUtility.TransitionTime, () => OnMenuDisable?.Invoke());
    }
}
