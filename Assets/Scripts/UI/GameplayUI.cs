using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : Singleton<GameplayUI>
{
    public IntVariable score;
    public IntVariable coin;

    [Header("Play Panel")]
    public GameObject playPanel;
    public TMP_Text p_score;
    public Slider p_health;
    public TMP_Text p_health_text;
    public TMP_Text p_coin;
    public Slider p_event;

    [Header("Gameover Panel")]
    public GameObject gameoverPanel;
    public TMP_Text g_score;
    public TMP_Text g_highScore;

    public enum Panel { Play, Gameover, None }
    private Panel panel;

    void Update()
    {
        p_score.text = "score:" + score.Value;
        g_score.text = "score:" + score.Value;
        g_highScore.text = "highscore:" + score.previousHighestValue;
        p_coin.text = ":" + coin.Value;
    }

    public void SetScore()
    {
        p_score.text = "score:" + score.Value;
        g_score.text = "score:" + score.Value;
    }

    public void SetHighScore()
    {
        g_highScore.text = "highscore:" + score.previousHighestValue;
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

    public void SetCoin(int coin)
    {
        p_coin.text = ":" + coin;
    }

    public void SetEventSlider(float eventRatio)
    {
        p_event.value = eventRatio;
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

    public void OnGameover()
    {
        StartCoroutine(OnGameoverIE());
    }

    IEnumerator OnGameoverIE()
    {
        yield return new WaitForSeconds(1.6f);

        // timescale = 0
        Time.timeScale = 0f;

        // show gameover
        gameoverPanel.SetActive(true);
    }
}
