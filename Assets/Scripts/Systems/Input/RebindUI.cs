using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _inputActionReference;

    [SerializeField]
    private bool _excludeMouse = true;
    [SerializeField] 
    private bool _allowDuplicate = true;
    [Range(0, 10)]
    [SerializeField]
    private int _selectedBinding;
    [SerializeField]
    private InputBinding.DisplayStringOptions _displayStringOptions;
    [Header("Binding Info - DO NOT EDIT")]
    [SerializeField]
    private InputBinding _inputBinding;
    private int _bindingIndex;

    private string _actionName;

    [Header("UI Fields")]
    [SerializeField]
    private TextMeshProUGUI _actionText;
    public Button RebindButton;
    public TextMeshProUGUI RebindText;
    [SerializeField]
    private Button _resetButton;

    public UnityEvent<RebindUI, string, string, string> UpdateBindingUIEvent;

    private void OnEnable()
    {
        RebindButton.onClick.AddListener(() => StartRebind());
        _resetButton.onClick.AddListener(() => ResetToDefault());

        if (_inputActionReference != null)
        {
            InputRebinder.Instance.LoadBindingOverride(_actionName);
            RetrieveBindingInformation();
            UpdateDisplay();
        }

        InputRebinder.Instance.RebindComplete += UpdateDisplay;
        InputRebinder.Instance.RebindCanceled += UpdateDisplay;
    }

    private void OnDisable()
    {
        InputRebinder.Instance.RebindComplete -= UpdateDisplay;
        InputRebinder.Instance.RebindCanceled -= UpdateDisplay;
    }

    private void OnValidate()
    {
        if (_inputActionReference == null)
            return;

        RetrieveBindingInformation();
        UpdateDisplay();
    }

    private void RetrieveBindingInformation()
    {
        if (_inputActionReference.action != null)
            _actionName = _inputActionReference.action.name;

        if (_inputActionReference.action.bindings.Count > _selectedBinding)
        {
            _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
            _bindingIndex = _selectedBinding;
        }
    }

    public void UpdateDisplay()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        if (_inputActionReference.action != null)
        {
            displayString = _inputActionReference.action.GetBindingDisplayString(_bindingIndex, out deviceLayoutName, out controlPath);
        }

        if (_actionText != null)
        {
            _actionText.text = _actionName;
        }

        if (RebindText != null)
        {
            if (Application.isPlaying)
            {
                RebindText.text = InputRebinder.Instance.GetBindingName(_actionName, _bindingIndex);
            }
            else
                RebindText.text = displayString;
        }

        UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
    }

    private void StartRebind()
    {
        InputRebinder.Instance.StartInteractiveRebind(_actionName, _bindingIndex, RebindText, _excludeMouse, _allowDuplicate);
    }

    private void ResetToDefault()
    {
        InputRebinder.Instance.ResetToDefault(_actionName, _bindingIndex);
        UpdateDisplay();
    }
}