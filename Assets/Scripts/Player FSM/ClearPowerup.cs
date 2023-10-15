using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearPowerup")]
public class ClearPowerupAction : Action
{
    public override void Act(StateController controller)
    {
        PlayerStateController m = (PlayerStateController)controller;
        m.currentPowerupType = PowerupType.Default;
        Player.instance.playerGun.SetGun(GunType.None);
    }
}