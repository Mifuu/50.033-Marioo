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
        gameplayActions = playerActions.Gameplay;

        // gameplayActions.Jump.performed += ctx => movement.Jump(ctx);
    }

    void FixedUpdate()
    {
        // Debug.Log(gameplayActions.Move.ReadValue<float>());
        // movement.ProcessInputX(gameplayActions.Move.ReadValue<float>());
    }
}
