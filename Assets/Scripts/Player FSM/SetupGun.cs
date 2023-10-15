using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupGun")]
public class SetupGun : Action
{
    public GunType gunType;
    public override void Act(StateController controller)
    {
        Player.instance.playerGun.SetGun(gunType);
    }
}