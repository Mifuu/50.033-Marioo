using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public IntVariable gameScore;
    public IntVariable coin;
    private float eventTime = 300;
    private float time;

    [Header("UIs")]
    public TextMeshProUGUI scoreText;
    public GameObject gameoverPanel;
    public TextMeshProUGUI gameoverScoreText;
    public TextMeshProUGUI gameoverText;

    [Header("Enemies Parent Ref")]
    public GameObject enemies;

    public bool isGameover = false;

    // restart
    public delegate void OnRestart();
    public OnRestart onRestart;

    void Update()
    {
        time += Time.deltaTime;
        UpdateEventSlider();
    }

    void Start()
    {
        // reset UI
        gameoverPanel.SetActive(false);
        gameScore.Value = 0;
        // UpdateScoreText();
        coin.Value = 0;
        // UpdateCoinText();
        time = 0;
        UpdateEventSlider();

        Application.targetFrameRate = 60;

        //ResetGame();
    }

    public void AddScore(int add = 1)
    {
        gameScore.ApplyChange(add);
        // UpdateScoreText();
    }

    public void AddCoin(int add = 1)
    {
        coin.ApplyChange(add);
    }

    void UpdateScoreText()
    {
        // GameplayUI.instance.SetScore(gameScore.Value);
        // GameplayUI.instance.SetHighScore(gameScore.previousHighestValue);
    }

    public void UpdateHealthUI(int health, int maxHealth)
    {
        GameplayUI.instance.SetMaxHealth(maxHealth);
        GameplayUI.instance.SetHealth(health);
    }

    void UpdateCoinText()
    {
        GameplayUI.instance.SetCoin(coin.Value);
    }

    void UpdateEventSlider()
    {
        float eventRatio = time / eventTime;
        eventRatio = Mathf.Clamp(eventRatio, 0, 1);
        GameplayUI.instance.SetEventSlider(eventRatio);
    }

    public void RestartButtonCallback()
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        Time.timeScale = 1;
        isGameover = false;

        // onRestart.Invoke();
        ChangeSceneManager.instance.ChangeSceneFade("SampleScene 1");
    }

    public void GameOver()
    {
        if (isGameover == true) return;
        isGameover = true;

        StartCoroutine(GameOverIE());
    }

    IEnumerator GameOverIE()
    {
        SFXManager.instance.PlaySFX("mario_dead", gameObject);

        yield return new WaitForSeconds(0.6f);

        // knock player up
        Player.instance.playerMovement.Knock(Vector2.up * 24f);

        yield return new WaitForSeconds(1.0f);

        // timescale = 0
        Time.timeScale = 0f;

        // show gameover
        gameoverText.text = "Wasted";
        gameoverScoreText.text = "score:" + gameScore.Value;
        gameoverPanel.SetActive(true);
    }

    public bool IsGameover()
    {
        return isGameover;
    }
}
