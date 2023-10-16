using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Transform shotgunchiTransform;
    [SerializeField] Transform nijigunTransform;
    [SerializeField] Transform kitagunTransform;
    [SerializeField] Transform ryogunTransform;

    [SerializeField] private GunType gunType;
    public GunType GunType
    {
        get { return gunType; }
        set
        {
            if (value != gunType)
            {
                SetGun(value);
            }
        }
    }

    public void SetGun(GunType g)
    {
        shotgunchiTransform.gameObject.SetActive(false);
        nijigunTransform.gameObject.SetActive(false);
        kitagunTransform.gameObject.SetActive(false);
        ryogunTransform.gameObject.SetActive(false);

        switch (g)
        {
            case GunType.Shotgunchi:
                shotgunchiTransform.gameObject.SetActive(true);
                break;
            case GunType.Nijigun:
                nijigunTransform.gameObject.SetActive(true);
                break;
            case GunType.Kitagun:
                kitagunTransform.gameObject.SetActive(true);
                break;
            case GunType.Ryogun:
                ryogunTransform.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        gunType = g;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // SetGun(GunType.Shotgunchi);
            Player.instance.stateController.SetPowerup(PowerupType.Shotgunchi);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // SetGun(GunType.Nijigun);
            Player.instance.stateController.SetPowerup(PowerupType.Nijigun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // SetGun(GunType.Kitagun);
            Player.instance.stateController.SetPowerup(PowerupType.Kitagun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // SetGun(GunType.Ryogun);
            Player.instance.stateController.SetPowerup(PowerupType.Ryogun);
        }
    }
}

public enum GunType
{
    None,
    Shotgunchi,
    Nijigun,
    Kitagun,
    Ryogun
}