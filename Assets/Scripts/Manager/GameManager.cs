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

        // reset UI
        gameoverPanel.SetActive(false);

        // reset player data
        Player.instance.ResetPos();
        // reset flipping

        score = 0;
        scoreText.text = "Score:0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.GetComponent<EnemyMovement>().Reset();
        }
        // reset score
        // jumpOverGoomba.score = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        if (isGameover == true) return;

        isGameover = true;

        // show gameover
        gameoverText.text = "Gameover";
        gameoverScoreText.text = "score:" + score;
        gameoverPanel.SetActive(true);
    }
}
