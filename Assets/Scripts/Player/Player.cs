using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    Vector3 startPos;

    [Header("Requirements")]
    public PlayerMovement playerMovement;
    public JumpOverGoomba jumpOverGoomba;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        SetStartPos();
    }

    void Start()
    {

    }

    public void SetStartPos()
    {
        startPos = transform.position;
    }

    public void ResetPos()
    {
        transform.position = startPos;
    }
}
