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

    public Rigidbody2D rb;

    [HideInInspector]
    public bool isDead = false;

    public bool autoStartPosition = true;
    public Vector3 startPosition;
    private Vector3 startScale;

    public Transform rendererTransform;
    public GameObject transformParticle;

    public GameObject uoomba;

    void Awake()
    {
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

    public void Reset()
    {
        transform.position = startPosition;

        isDead = false;
        rendererTransform.localScale = startScale;
    }

    public void Dead()
    {
        isDead = true;
        rendererTransform.localScale = new Vector3(startScale.x, startScale.y * 0.6f, startScale.z);
        StartCoroutine(Transform());
    }

    IEnumerator Transform()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(transformParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Instantiate(transformParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(transformParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.3f);
        Instantiate(transformParticle, transform.position, Quaternion.identity);
        Instantiate(uoomba, transform.position, Quaternion.identity);
        uoomba.GetComponent<UoombaScript>().Knock(Vector2.up * 1);
        Destroy(gameObject);
    }

    public void Knock(Vector2 velo)
    {
        // Vector2 velocity = rb.velocity;
        rb.velocity = velo;
        // rb.velocity = velocity;
    }
}