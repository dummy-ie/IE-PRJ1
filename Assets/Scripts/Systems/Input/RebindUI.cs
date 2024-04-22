using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField]
    private Button _rebindButton;
    [SerializeField]
    private TextMeshProUGUI _rebindText;
    [SerializeField]
    private Button _resetButton;

    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(() => StartRebind());
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

    private void UpdateDisplay()
    {
        if (_actionText != null)
            _actionText.text = _actionName;

        if (_rebindText != null)
        {
            if (Application.isPlaying)
            {
                _rebindText.text = InputRebinder.Instance.GetBindingName(_actionName, _bindingIndex);
            }
            else
                _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
        }
    }

    private void StartRebind()
    {
        InputRebinder.Instance.StartInteractiveRebind(_actionName, _bindingIndex, _rebindText, _excludeMouse, _allowDuplicate);
    }

    private void ResetToDefault()
    {
        InputRebinder.Instance.ResetToDefault(_actionName, _bindingIndex);
        UpdateDisplay();
    }
}