using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonController : MonoBehaviour, IInteractiveButton
{
    public void ButtonClick()
    {
        Debug.Log("On click menu button");
        Time.timeScale = 1;
        ChangeSceneManager.instance.ChangeSceneFade("MainMenu");
    }
}
