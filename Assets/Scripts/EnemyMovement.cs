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

    private Rigidbody2D enemyBody;

    [HideInInspector]
    public bool isDead = false;

    public bool autoStartPosition = true;
    public Vector3 startPosition;
    private Vector3 startScale;

    void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
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
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!isDead)
        {
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
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

    public void Reset()
    {
        transform.position = startPosition;
    }

    public void Dead()
    {
        isDead = true;
        transform.localScale = new Vector3(startScale.x, startScale.y * 0.3f, startScale.z);
    }
}