using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10;
    public float maxSpeed = 20;

    [Header("Jump Settings")]
    public float upSpeed = 10;
    public float upSpeedLiftKeyFactor = 0.5f;
    public float coyoteTime = 0.1f;
    public float groundCheckDist = 0.1f;
    public LayerMask groundMask;
    private bool onGroundState = true;
    private float timeSinceGround = 0;

    [Header("Enemy")]
    public LayerMask enemyMask;

    // for flipping sprite
    private SpriteRenderer sr;
    private bool faceRightState = true;

    public JumpOverGoomba jumpOverGoomba;
    public Rigidbody2D rb;
    new public BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollideEnemy();
        GroundUpdate();
        MoveUpdate();
        FlipSpriteUpdate();
    }

    void CheckCollideEnemy()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, collider.bounds.size, 0, transform.up, 0, enemyMask);
        foreach (var hit in hits)
        {
            if (hit && !hit.transform.GetComponent<EnemyMovement>().isDead)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    void GroundUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, collider.bounds.size, 0, -transform.up, groundCheckDist, groundMask);
        if (hit)
        {
            onGroundState = true;
        }
        else
        {
            onGroundState = false;
        }
    }

    void MoveUpdate()
    {
        if (onGroundState)
        {
            timeSinceGround = 0;
        }
        else
        {
            timeSinceGround += Time.deltaTime;
        }

        float velocityX = Input.GetAxisRaw("Horizontal");
        velocityX *= speed;

        float velocityY = rb.velocity.y;

        // if jump
        if (Input.GetKeyDown(KeyCode.Space) && timeSinceGround < coyoteTime)
        {
            velocityY = Jump();
        }

        // potentially decrese y speed when release (not fool proof)
        if (Input.GetKeyUp(KeyCode.Space) && !onGroundState && velocityY > upSpeed * upSpeedLiftKeyFactor)
        {
            velocityY = upSpeed * upSpeedLiftKeyFactor;
        }

        if (!onGroundState && velocityY < 0)
        {
            velocityY -= 15 * Time.deltaTime;
        }

        rb.velocity = new Vector2(velocityX, velocityY);
    }

    public float Jump()
    {
        return Jump(1);
    }

    public float Jump(float fac)
    {
        float velocityY = rb.velocity.y;
        velocityY = upSpeed * fac;
        rb.velocity = new Vector2(rb.velocity.x, velocityY);

        return velocityY;
    }

    void FlipSpriteUpdate()
    {
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            sr.flipX = false;
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            sr.flipX = true;
        }
    }
}