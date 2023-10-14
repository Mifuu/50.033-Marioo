using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UoombaScript : Damageable
{
    [Space]
    [Header("Uoomba")]
    public Rigidbody2D rb;

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

    protected override void Awake()
    {
        base.Awake();

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

        if (HP <= 0) rb.gravityScale = gravity;
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

    public void Reset()
    {
        transform.position = startPosition;

        HP = maxHP;
        rendererTransform.localScale = startScale;
    }

    public override void TakeDamage(Damage dmg, Vector2 dir)
    {
        SFXManager.TryPlaySFX("mon_damage_1", gameObject);

        Knock(dmg.knockFac * dir.normalized);

        base.TakeDamage(dmg, dir);
    }

    protected override void Dead(Damage dmg, Vector2 dir)
    {
        SFXManager.TryPlaySFX("mon_dead_1", gameObject);

        if (dmg.isHeavy)
        {
            DetachLegs(dmg.knockFac * dir);
        }

        Knock(dmg.knockFac * dir);
        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range(-180, 180);

        base.Dead(dmg, dir);
    }

    public void Knock(Vector2 velo)
    {
        // Vector2 velocity = rb.velocity;
        rb.velocity = velo;
        // rb.velocity = velocity;
    }

    private void DetachLegs(Vector2 velocity)
    {
        foreach (var l in legs)
        {
            l.Detach(velocity);
        }
    }
}