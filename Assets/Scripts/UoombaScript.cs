using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UoombaScript : MonoBehaviour
{


    public Rigidbody2D rb;

    [HideInInspector]
    public bool isDead = false;

    public bool autoStartPosition = true;
    public Vector3 startPosition;
    private Vector3 startScale;

    public Transform rendererTransform;

    [Header("Goomba Idle")]
    public float G_I_time = 1.5f;
    public float G_I_speed = 4f;
    public bool G_I_goRight = true;

    [Header("Legs")]
    public Leg[] legs;
    private int legContactNum;
    public float gravityFacDecreasePerLeg;
    private float gravity;

    void Awake()
    {
        // get the starting position
        if (autoStartPosition)
        {
            startPosition = transform.position;
        }
        startScale = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(GoombaIdleIE());
        gravity = rb.gravityScale;
    }

    void Update()
    {
        legContactNum = 0;
        foreach (var l in legs)
        {
            if (l.contact) legContactNum++;
        }

        rb.gravityScale = gravity * (1 - (legContactNum * gravityFacDecreasePerLeg));
        Debug.Log(rb.gravityScale);
    }

    IEnumerator GoombaIdleIE()
    {
        float timer = G_I_time * Random.Range(0.4f, 1.5f);
        while (timer > 0)
        {
            rb.velocity = new Vector2(G_I_speed * (G_I_goRight ? 1 : -1), rb.velocity.y);
            timer -= Time.deltaTime;
            yield return null;
        }
        G_I_goRight = !G_I_goRight;
        StartCoroutine(GoombaIdleIE());
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
        rendererTransform.localScale = startScale;
    }

    public void Dead()
    {
        isDead = true;
        rendererTransform.localScale = new Vector3(startScale.x, startScale.y * 0.6f, startScale.z);
    }

    public void Knock(Vector2 velo)
    {
        // Vector2 velocity = rb.velocity;
        rb.velocity = velo;
        // rb.velocity = velocity;
    }
}