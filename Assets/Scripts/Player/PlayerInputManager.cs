using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerActions playerActions;
    public PlayerActions.GameplayActions gameplayActions;

    public PlayerMovement movement;

    void Awake()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        gameplayActions = playerActions.Gameplay;

        gameplayActions.Jump.performed += movement.OnJump;
        gameplayActions.Jump.canceled += movement.OnJump;
        gameplayActions.Move.performed += movement.OnHorizontalInput;
        gameplayActions.Move.canceled += movement.OnHorizontalInput;
    }
}
