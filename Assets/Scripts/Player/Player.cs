using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            {
                // GameManager.instance.UpdateHealthUI(health, maxHealth);
                onSetHP.Invoke(health);
                onSetMaxHP.Invoke(gc.player.maxHealth);
            }
        }
    }
    private int maxHealth;
    private float iTime;
    public bool Alive
    {
        get { return Health > 0; }
    }
    public UnityEvent onGameOver;
    public UnityEvent<int> onSetHP;
    public UnityEvent<int> onSetMaxHP;
    bool isGameover = false;

    [Header("Requirements")]
    public PlayerMovement playerMovement;
    public JumpOverGoomba jumpOverGoomba;
    public PlayerAnim playerAnim;

    [Header("Shotgun")]
    public GameObject shotgun;
    public PlayerGun playerGun;

    [Header("FSM")]
    public PlayerStateController stateController;

    void Start()
    {
        maxHealth = gc.player.maxHealth;
        Health = maxHealth;
    }

    void Update()
    {
        iTime -= Time.deltaTime;
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
        stateController.SetPowerup(PowerupType.Damage);
        CameraShake.instance.Shake(0.5f);

        if (Health < 1)
        {
            if (isGameover == true) return;
            {
                isGameover = true;
                onGameOver.Invoke();
                StartCoroutine(OnGameover());
            }
        }
    }

    public void EnableShotgun(bool value)
    {
        shotgun.SetActive(value);
    }

    IEnumerator OnGameover()
    {
        SFXManager.instance.PlaySFX("mario_dead", gameObject);

        yield return new WaitForSeconds(0.6f);

        // knock player up
        Player.instance.playerMovement.Knock(Vector2.up * 24f);

        yield return new WaitForSeconds(1.0f);
    }
}
