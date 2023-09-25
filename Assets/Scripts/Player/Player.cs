using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    Vector3 startPos;

    [Header("State")]
    public bool alive = true;

    [Header("Requirements")]
    public PlayerMovement playerMovement;
    public JumpOverGoomba jumpOverGoomba;
    public PlayerAnim playerAnim;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        SetStartPos();
        Reset();
    }

    void Start()
    {

    }

    void OnEnable()
    {
        GameManager.instance.onRestart += Reset;
    }

    void OnDisable()
    {
        GameManager.instance.onRestart -= Reset;
    }

    public void Reset()
    {
        alive = true;
        playerAnim.Restart();
        transform.position = startPos;
    }

    public void SetStartPos()
    {
        startPos = transform.position;
    }
}
