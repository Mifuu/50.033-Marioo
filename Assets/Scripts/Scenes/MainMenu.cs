using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public IntVariable highScore;
    public TMP_Text highScoreText;

    void Awake()
    {
        highScoreText.text = "Hi: " + highScore.previousHighestValue;
    }

    public void StartCallback()
    {
        ChangeSceneManager.instance.ChangeSceneFade();
    }

    public void QuitCallback()
    {
        Application.Quit();
    }

    public void ResetCallback()
    {
        highScore.ResetHighestValue();
        highScoreText.text = "Hi: " + highScore.previousHighestValue;
    }
}
