using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// A Scriptable Singleton that handles events on player input.
/// </summary>
[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Singletons/InputReader")]
public class InputReader : ScriptableSingleton<InputReader>, Controls.IGameplayActions, GameInitializer.IInitializableSingleton//, Controls.IUIActions
{
    public void Initialize()
    {
        InputReader instance = Instance;
    }

    public Controls InputActions { get; private set; }

    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpEventCanceled;
    public event Action DashEvent;
    public event Action InteractEvent;
    public event Action InvisibilityEvent;
    public event Action PauseEvent;

    private void OnEnable()
    {
        if (InputActions == null)
        {
            InputActions = new Controls();
            InputActions.Gameplay.SetCallbacks(this);
        }
        InputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        InputActions.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        InputActions.UI.Disable();
        InputActions.Gameplay.Enable();
    }

    public void DisableAllInput() {
        InputActions.Gameplay.Disable();
        InputActions.UI.Disable();
    }

    #region GameplayInput
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                JumpEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                JumpEventCanceled?.Invoke();
                break;
        }
    }

    void Controls.IGameplayActions.OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            DashEvent?.Invoke();
    }

    void Controls.IGameplayActions.OnAttack(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnVectorShift(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnChargedThrust(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnManiteSlash(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            InteractEvent?.Invoke();
    }

    void Controls.IGameplayActions.OnSubmit(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnGroundPound(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnAbility1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            InvisibilityEvent?.Invoke();
    }

    void Controls.IGameplayActions.OnAbility2(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnParry(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnAbilityCycle(InputAction.CallbackContext context)
    {
    }

    void Controls.IGameplayActions.OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            PauseEvent?.Invoke();
    }
    #endregion
}
