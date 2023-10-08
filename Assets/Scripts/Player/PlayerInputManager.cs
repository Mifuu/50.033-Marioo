using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerActions playerActions;
    public PlayerActions.GameplayActions gameplayActions;

    public PlayerMovement movement;

    public bool active = true;
    private bool subscribed = false;

    void Awake()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        gameplayActions = playerActions.Gameplay;
    }

    void Update()
    {
        // update
        if (active && !subscribed) Subscribe();
        if (!active && subscribed) Unsubscribe();
    }

    void OnEnable()
    {
        if (active) Subscribe();
    }

    void OnDisable()
    {
        if (subscribed) Unsubscribe();
    }

    void Subscribe()
    {
        subscribed = true;
        gameplayActions.Jump.performed += movement.OnJump;
        gameplayActions.Jump.canceled += movement.OnJump;
        gameplayActions.Move.performed += movement.OnHorizontalInput;
        gameplayActions.Move.canceled += movement.OnHorizontalInput;
    }

    void Unsubscribe()
    {
        subscribed = false;
        gameplayActions.Jump.performed -= movement.OnJump;
        gameplayActions.Jump.canceled -= movement.OnJump;
        gameplayActions.Move.performed -= movement.OnHorizontalInput;
        gameplayActions.Move.canceled -= movement.OnHorizontalInput;
    }
}
