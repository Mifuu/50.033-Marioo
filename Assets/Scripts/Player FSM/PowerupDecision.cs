using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Gun")]
public class PowerupDecision : Decision
{
    public StateTransformMap[] map;

    public override bool Decide(StateController controller)
    {
        PlayerStateController p = (PlayerStateController)controller;
        // we assume that the state is named (string matched) after one of possible values in MarioState
        // convert between current state name into MarioState enum value using custom class EnumExtension
        // you are free to modify this to your own use
        PlayerState toCompareState = EnumExtension.ParseEnum<PlayerState>(p.currentState.name);

        // loop through state transform and see if it matches the current transformation we are looking for
        for (int i = 0; i < map.Length; i++)
        {
            bool stateCheck = map[i].fromAnyState ? true : toCompareState == map[i].fromState;
            if (stateCheck && p.currentPowerupType == map[i].powerupCollected)
            {
                return true;
            }
        }

        return false;

    }
}

[System.Serializable]
public struct StateTransformMap
{
    public bool fromAnyState;
    public PlayerState fromState;
    public PowerupType powerupCollected;
}