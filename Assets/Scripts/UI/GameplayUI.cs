using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : Singleton<GameplayUI>
{
    [Header("Play Panel")]
    public GameObject playPanel;
    public TMP_Text p_score;
    public Slider p_health;
    public TMP_Text p_health_text;

    [Header("Gameover Panel")]
    public GameObject gameoverPanel;
    public TMP_Text g_score;
    public TMP_Text g_highScore;

    public enum Panel { Play, Gameover, None }
    private Panel panel;

    public void SetScore(int score)
    {
        p_score.text = "score:" + score;
        g_score.text = "score:" + score;
    }

    public void SetHighScore(int highScore)
    {
        g_highScore.text = "highscore:" + highScore;
    }

    public void SetHealth(int health)
    {
        p_health.value = health;
        p_health_text.text = "HP " + p_health.value + "/" + p_health.maxValue;
    }

    public void SetMaxHealth(int maxHealth)
    {
        p_health.maxValue = maxHealth;
    }

    public void SetPanel(Panel panel)
    {
        DisableAllPanel();

        switch (panel)
        {
            case Panel.Play:
                playPanel.SetActive(true);
                break;
            case Panel.Gameover:
                gameoverPanel.SetActive(true);
                break;
            case Panel.None:
                break;
        }
    }

    public void DisableAllPanel()
    {
        playPanel.SetActive(false);
        gameoverPanel.SetActive(false);
    }
}
