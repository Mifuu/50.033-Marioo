using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : StateController
{
    public PowerupType currentPowerupType = PowerupType.Default;
    public PlayerState shouldBeNextState = PlayerState.Default;

    public override void Start()
    {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }

    public void GameRestart()
    {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i)
    {
        currentPowerupType = i;
    }

    public SpriteRenderer spriteRenderer;
    public void SetRendererToFlicker()
    {
        StartCoroutine(BlinkSpriteRenderer());
    }
    private IEnumerator BlinkSpriteRenderer()
    {
        while (string.Equals(currentState.name, "StateInvincible", StringComparison.OrdinalIgnoreCase))
        {
            // Toggle the visibility of the sprite renderer
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(0.15f);
        }

        spriteRenderer.enabled = true;
    }
}

public enum PlayerState
{
    Default = -1,
    Normal = 0,
    Shotgunchi = 1,
    Nijigun = 2,
    Kitagun = 3,
    Ryogun = 4,
    Invincible = 5,
    DeadPlayer = 99
}

public enum PowerupType
{
    Shotgunchi = 1,
    Nijigun = 2,
    Kitagun = 3,
    Ryogun = 4,
    Damage = 99,
    Default = -1
}
