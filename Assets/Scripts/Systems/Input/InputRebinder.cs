using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.HDROutputUtils;

/// <summary>
/// A Scriptable Singleton that handles events on rebinding controls.
/// </summary>
[CreateAssetMenu(fileName = "InputRebinder", menuName = "Scriptable Singletons/InputRebinder")]
public class InputRebinder : ScriptableSingleton<InputRebinder>, GameInitializer.IInitializableSingleton
{
    public void Initialize()
    {
        InputRebinder instance = Instance;
        InputActions = InputReader.Instance.InputActions;
    }
    public Controls InputActions { get; private set; }

    public event Action RebindComplete;
    public event Action RebindCanceled;
    public event Action<InputAction, int> RebindStarted;

    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    public void StartInteractiveRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse, bool allowDuplicate)
    {
        InputAction action = InputActions.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                PerformInteractiveRebind(action, firstPartIndex, statusText, excludeMouse, allowDuplicate, true);
        }
        else
            PerformInteractiveRebind(action, bindingIndex, statusText, excludeMouse, allowDuplicate);
    }

    private void PerformInteractiveRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse, bool allowDuplicate, bool allCompositeParts = false)
    {
        Debug.Log("Do Rebind: " + actionToRebind.name);
        if (actionToRebind == null || bindingIndex < 0)
            return;

        void CleanUp()
        {
            _rebindOperation.Dispose();
            _rebindOperation = null;
        }

        actionToRebind.Disable();

        _rebindOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .OnCancel(
                operation =>
                {
                    actionToRebind.Enable();
                    RebindCanceled?.Invoke();
                    CleanUp();
                })
            .OnComplete(
                operation =>
                {
                    actionToRebind.Enable();

                    // If there's a duplicate binding, redo the binding
                    if (!allowDuplicate)
                    {
                        if (CheckDuplicateBindings(actionToRebind, bindingIndex, allCompositeParts))
                        {
                            actionToRebind.RemoveBindingOverride(bindingIndex);
                            CleanUp();
                            PerformInteractiveRebind(actionToRebind, bindingIndex, statusText, excludeMouse, allowDuplicate, allCompositeParts);
                            return;
                        }
                    }

                    // If there's a duplicate binding on the current composite part,
                    // redo the binding
                    if (CheckCompositeDuplicateBindings(actionToRebind, bindingIndex, allCompositeParts))
                    {
                        actionToRebind.RemoveBindingOverride(bindingIndex);
                        CleanUp();
                        PerformInteractiveRebind(actionToRebind, bindingIndex, statusText, excludeMouse, allowDuplicate, allCompositeParts);
                        return;
                    }

                    SaveBindingOverride(actionToRebind);
                    RebindComplete?.Invoke();
                    CleanUp();

                    // If there's more composite parts to bind, initiate a rebind
                    // for the next part.
                    if (allCompositeParts)
                    {
                        var nextBindingIndex = bindingIndex + 1;
                        if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
                            PerformInteractiveRebind(actionToRebind, nextBindingIndex, statusText, excludeMouse, allowDuplicate, allCompositeParts);
                    }
                })
            .WithCancelingThrough("<Keyboard>/escape");
        if (excludeMouse)
            _rebindOperation.WithControlsExcluding("Mouse");

        // If it's a part binding, show the name of the part in the UI.
        var partName = default(string);
        if (actionToRebind.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{actionToRebind.bindings[bindingIndex].name}'. ";

        statusText.text = $"{partName}Waiting for input...";

        // Starts the rebinding process and invokes listeners.
        RebindStarted?.Invoke(actionToRebind, bindingIndex);
        
        _rebindOperation.Start(); 
    }

    private bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        InputBinding newBinding = action.bindings[bindingIndex];
        foreach (InputBinding binding in action.actionMap.bindings)
        {
            if (binding.action == newBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == newBinding.effectivePath)
            {
                Debug.Log("Duplicate binding found: " + newBinding.effectivePath);
                return true;
            }
        }
        return CheckCompositeDuplicateBindings(action, bindingIndex, allCompositeParts);
    }

    private bool CheckCompositeDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        InputBinding newBinding = action.bindings[bindingIndex];
        if (allCompositeParts)
        {
            for (int i = 1; i < bindingIndex; i++)
            {
                if (action.bindings[i].effectivePath == newBinding.overridePath)
                {
                    Debug.Log("Duplicate binding found: " + newBinding.effectivePath);
                    return true;
                }
            }
        }
        return false;
    }

    public string GetBindingName(string actionName, int bindingIndex)
    {
        if (InputActions == null)
            InputActions = InputReader.Instance.InputActions;

        InputAction action = InputActions.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    private void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public void LoadBindingOverride(string actionName)
    {
        if (InputActions == null)
            InputActions = InputReader.Instance.InputActions;

        InputAction action = InputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
        }
    }

    public void ResetToDefault(string actionName, int bindingIndex)
    {
        InputAction action = InputActions.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isPartOfComposite; i++)
                action.RemoveBindingOverride(i);
        }
        else
            action.RemoveBindingOverride(bindingIndex);

        SaveBindingOverride(action);
    }
}
