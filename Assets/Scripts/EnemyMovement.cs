using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D rb;

    [HideInInspector]
    public bool isDead = false;

    public bool autoStartPosition = true;
    public Vector3 startPosition;
    private Vector3 startScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();

        if (autoStartPosition)
        {
            startPosition = transform.position;
        }
        startScale = transform.localScale;
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        rb.velocity = velocity;
        // rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!isDead)
        {
            if (Mathf.Abs(rb.position.x - originalX) < maxOffset)
            {// move goomba
                Movegoomba();
            }
            else
            {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                Movegoomba();
            }
        }
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
        transform.position = startPosition;

        isDead = false;
        transform.localScale = startScale;
    }

    public void Dead()
    {
        isDead = true;
        transform.localScale = new Vector3(startScale.x, startScale.y * 0.6f, startScale.z);
    }

    public void Knock(Vector2 velo)
    {
        // Vector2 velocity = rb.velocity;
        rb.velocity = velo;
        // rb.velocity = velocity;
    }
}