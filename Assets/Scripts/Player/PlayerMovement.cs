using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10;
    public float maxSpeed = 20;
    private float currentVelocityX;
    public float smoothTime = 0.5f;

    [Header("Wall Stuck Fix Setting")]
    [Range(2, 10)]
    public int wsRaycastCount = 3;
    public float wsCheckDist = 0.1f;
    public LayerMask wsLayerMask;

    [Header("Jump Settings")]
    public float upSpeed = 10;
    public float upSpeedLiftKeyFactor = 0.5f;
    public float coyoteTime = 0.1f;
    public float groundCheckDist = 0.1f;
    public LayerMask groundMask;
    public bool onGround = true;
    private float timeSinceGround = 0;

    [Header("Enemy")]
    public LayerMask enemyMask;

    // for flipping sprite
    public SpriteRenderer sr;
    private bool faceRightState = true;

    public JumpOverGoomba jumpOverGoomba;
    public Rigidbody2D rb;
    new public BoxCollider2D collider;
    public PlayerAnim playerAnim;

    [Header("Requirements")]
    public Player player;

    // input
    public float inputX;
    public void OnHorizontalInput(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<float>();

        if (inputX < 0)
            sr.flipX = true;
        if (inputX > 0)
            sr.flipX = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (timeSinceGround < coyoteTime)
                Jump();
        }
        else if (context.canceled)
        {
            float velocityY = rb.velocity.y;
            if (!onGround && velocityY > upSpeed * upSpeedLiftKeyFactor)
            {
                Jump(upSpeedLiftKeyFactor);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // sr = GetComponent<SpriteRenderer>();
        // collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSkid();
        CheckCollideEnemy();
        GroundUpdate();
        MoveUpdate();
    }

    void CheckCollideEnemy()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, collider.bounds.size + (2 * collider.edgeRadius) * Vector3.one, 0, transform.up, 0, enemyMask);
        foreach (var hit in hits)
        {
            if (hit && !hit.transform.GetComponent<EnemyMovement>().isDead)
            {
                player.alive = false;
                GameManager.instance.GameOver();
            }
        }
    }

    void CheckSkid()
    {
        if (!onGround) return;
        if (inputX > 0 && rb.velocity.x < 0)
            playerAnim.Skid();
        else if (inputX < 0 && rb.velocity.x > 0)
            playerAnim.Skid();
    }

    void GroundUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, collider.bounds.size + (2 * collider.edgeRadius) * Vector3.one, 0, -transform.up, groundCheckDist, groundMask);
        if (hit)
        {
            onGround = true;
            timeSinceGround = 0;
        }
        else
        {
            onGround = false;
            timeSinceGround += Time.deltaTime;
        }
    }

    void MoveUpdate()
    {
        if (!player.alive) return;

        float velocityX = rb.velocity.x;
        float _inputX = inputX * speed;
        if (Mathf.Abs(velocityX) < Mathf.Abs(_inputX) * 1.2f)
        {
            velocityX = _inputX;
        }

        // damping x velocity
        velocityX *= 0.9f;
        velocityX = Mathf.SmoothDamp(velocityX, 0, ref currentVelocityX, smoothTime);

        // fix stuck to walls
        if (velocityX != 0 && WillCollide(velocityX))
        {
            velocityX = 0;
        }

        float velocityY = rb.velocity.y;

        // faster falling acceleration
        if (!onGround && velocityY < 0)
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

    public void Knock(Vector2 velo)
    {
        rb.velocity = velo;
    }

    private bool WillCollide(float velocityX)
    {
        bool goRight = velocityX > 0;

        Bounds bounds = collider.bounds;
        bounds.Expand(collider.edgeRadius);

        Vector2 raycastOrigin;
        if (goRight)
        {
            raycastOrigin = new Vector2(bounds.max.x, bounds.min.y);
        }
        else
        {
            raycastOrigin = new Vector2(bounds.min.x, bounds.min.y);
        }

        float incrementY = (bounds.extents.y * 2) / (wsRaycastCount - 1);
        bool isHit = false;
        Vector2 dir = Mathf.Sign(velocityX) * Vector2.right;
        for (int i = 0; i < wsRaycastCount - 1; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, dir, wsCheckDist, wsLayerMask);
            if (hit)
            {
                isHit = true;
                break;
            }
            Debug.DrawRay(raycastOrigin, dir, Color.red);
            raycastOrigin += Vector2.up * incrementY;
        }

        return isHit;
    }
}