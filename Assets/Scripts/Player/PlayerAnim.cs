using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private bool onGround = false;
    private float moveSpeed = 0;
    private bool isDead = false;

    GameManager gm;
    public Player player;
    public PlayerMovement playerMovement;
    public Animator animator;

    void Start()
    {
        gm = GameManager.instance;
    }

    void Update()
    {
        onGround = playerMovement.onGround;
        moveSpeed = Mathf.Abs(playerMovement.rb.velocity.x);
        isDead = !player.alive;

        animator.SetBool("onGround", onGround);
        animator.SetFloat("moveSpeed", moveSpeed);
        animator.SetBool("isDead", isDead);
    }

    public void Skid()
    {
        animator.SetTrigger("skid");
    }

    public void Restart()
    {
        animator.SetTrigger("restart");
    }
}
