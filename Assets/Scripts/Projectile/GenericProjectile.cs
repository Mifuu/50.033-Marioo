using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YUtil;

public class GenericProjectile : MonoBehaviour
{
    [Header("Init Settings")]
    public Vector3 target;
    public float speed = 5;
    public Damage damage;
    public float lifeTime = 10;
    protected float time = 0;

    [Header("Target")]
    public LayerMask hitMask;

    [Header("Physics")]
    public Vector2 velocity;
    public Vector2 acceleration;
    public float speedRetainFac = 0.96f;
    public float angularSpeed = 0;

    [Header("Hit Effects")]
    public GameObject hitParticle;
    public string hitSFX;
    public IntVariable score;

    [Header("Homing")]
    public bool homingEnable;
    public enum HomingMode { Velocity, Rotation }
    public HomingMode homingMode;
    public float homingRadius;
    public float homingSpeed;
    public float homingDegreeSpeed;

    [Header("Requirements")]
    new public Collider2D collider;
    public SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        velocity = ((Vector2)target - (Vector2)transform.position).normalized * speed;
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        // homing update
        UpdateHoming();

        // apply movement
        velocity *= speedRetainFac;
        velocity += acceleration * Time.fixedDeltaTime;
        Vector3 pos = transform.position;
        pos.x += velocity.x * Time.fixedDeltaTime;
        pos.y += velocity.y * Time.fixedDeltaTime;
        transform.position = pos;

        // apply rotation
        transform.Rotate(Vector3.forward, angularSpeed * Time.fixedDeltaTime);

        // life time
        if (time > lifeTime)
        {
            DestroyProjectile();
        }
    }

    void UpdateHoming()
    {
        if (!homingEnable) return;

        // get target
        Damageable closest = null;
        float sqrClosestDst = float.MaxValue;
        foreach (var d in Damageable.instances)
        {
            if (d.IsDead()) continue;
            float _sqrDist = (d.transform.position - transform.position).sqrMagnitude;
            if (_sqrDist < sqrClosestDst)
            {
                closest = d;
                sqrClosestDst = _sqrDist;
            }
        }

        // if closest is too far
        if (sqrClosestDst > homingRadius * homingRadius)
            return;

        if (homingMode == HomingMode.Velocity)
        {
            // apply velocity
            velocity += ((Vector2)(closest.transform.position - transform.position)).normalized * homingSpeed * Time.fixedDeltaTime;
        }
        else
        {
            Vector2 ePos = closest.transform.position;

            float currentAngle = YUtil.AngPosUtil.GetAngle(Vector2.zero, velocity);
            float enemyAngle = YUtil.AngPosUtil.GetAngle(transform.position, ePos);

            float deltaAngle = enemyAngle - currentAngle;

            deltaAngle = Mathf.Clamp(deltaAngle, -homingDegreeSpeed * Time.fixedDeltaTime, homingDegreeSpeed * Time.fixedDeltaTime);

            float angle = (currentAngle + deltaAngle) * Mathf.Deg2Rad;
            float mag = velocity.magnitude;

            velocity.x = Mathf.Cos(angle) * mag;
            velocity.y = Mathf.Sin(angle) * mag;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsMaskMatched(collider.gameObject))
        {
            Damageable d = collider.gameObject.GetComponent<Damageable>();
            if (d)
            {
                d.TakeDamage(damage, target);
                score.ApplyChange(1);
            }
            DestroyProjectile();
        }
    }

    public virtual bool IsMaskMatched(GameObject target)
    {
        return ((hitMask.value & (1 << target.layer)) > 0);
    }

    protected virtual void DestroyProjectile()
    {
        StartCoroutine(DestroyProjectileIE());
    }

    protected virtual IEnumerator DestroyProjectileIE()
    {
        // effects
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        SFXManager.TryPlaySFX(hitSFX, gameObject);

        // disable
        spriteRenderer.enabled = false;
        collider.enabled = false;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        homingEnable = false;
        angularSpeed = 0;

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }
}
