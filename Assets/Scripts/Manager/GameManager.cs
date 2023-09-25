using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;

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

    void OnEnable()
    {
        onRestart += () =>
        {
            // reset UI
            gameoverPanel.SetActive(false);

            score = 0;
            scoreText.text = "Score:0";
        };
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 30;

        //ResetGame();
    }

    public void AddScore(int add = 1)
    {
        score += add;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score:" + score.ToString();
    }

    public void RestartButtonCallback(int input)
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

        onRestart.Invoke();
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
        gameoverScoreText.text = "score:" + score;
        gameoverPanel.SetActive(true);
    }

    public bool IsGameover()
    {
        return isGameover;
    }
}
