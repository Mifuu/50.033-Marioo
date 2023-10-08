using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonController : MonoBehaviour, IInteractiveButton
{
    public void ButtonClick()
    {
        Debug.Log("On click restart button");
        GameManager.instance.RestartButtonCallback();
    }
}
