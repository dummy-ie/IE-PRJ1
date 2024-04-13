using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.
[CreateAssetMenu(fileName = "InputManager", menuName = "Scriptable Singletons/InputManager")]
public class InputManager : ScriptableSingleton<InputManager>, StuckinBetween.IGameplayActions, GameInitializer.IInitializableSingleton//, StuckinBetween.IUIActions
{
    public void Initialize()
    {
        InputManager singleton = Instance;
    }
    public StuckinBetween InputActions { get; private set; }

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
            InputActions = new StuckinBetween();

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

    void StuckinBetween.IGameplayActions.OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            DashEvent?.Invoke();
    }

    void StuckinBetween.IGameplayActions.OnAttack(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnVectorShift(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnChargedThrust(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnManiteSlash(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            InteractEvent?.Invoke();
    }

    void StuckinBetween.IGameplayActions.OnSubmit(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnGroundPound(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnAbility1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            InvisibilityEvent?.Invoke();
    }

    void StuckinBetween.IGameplayActions.OnAbility2(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnParry(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnAbilityCycle(InputAction.CallbackContext context)
    {
    }

    void StuckinBetween.IGameplayActions.OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            PauseEvent?.Invoke();
    }
    #endregion

    //void StuckinBetween.IPlayerActions.OnMove(InputAction.CallbackContext context)
    //{
    //
    //}
    /*
    private Vector2 moveDirection = Vector2.zero;
    private bool jumpPressed = false;
    private bool interactPressed = false;
    private bool submitPressed = false;


    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetJumpPressed()
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
    */
}
