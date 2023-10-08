using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    Vector3 startPos;

    [Header("Health")]
    public GameConstants gc;
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (GameManager.instance != null)
                GameManager.instance.UpdateHealthUI(health, maxHealth);
        }
    }
    private int maxHealth;
    private float iTime;
    public bool Alive
    {
        get { return Health > 0; }
    }

    [Header("Requirements")]
    public PlayerMovement playerMovement;
    public JumpOverGoomba jumpOverGoomba;
    public PlayerAnim playerAnim;

    [Header("Shotgun")]
    public GameObject shotgun;

    void Start()
    {
        maxHealth = gc.player.maxHealth;
        Health = maxHealth;
    }

    void Update()
    {
        iTime -= Time.deltaTime;
    }

    void OnEnable()
    {
        if (GameManager.instance != null)
            GameManager.instance.onRestart += Reset;
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
            GameManager.instance.onRestart -= Reset;
    }

    public void Reset()
    {
        Health = maxHealth;
        playerAnim.Restart();
        transform.position = startPos;
    }

    public void SetStartPos()
    {
        startPos = transform.position;
    }

    public void TakeDamage()
    {
        if (iTime > 0) return;

        iTime = gc.player.dmgITime;
        Health -= 1;
        if (Health < 0) Health = 0;

        if (Health < 1) GameManager.instance.GameOver();
    }

    public void EnableShotgun(bool value)
    {
        shotgun.SetActive(value);
    }
}
