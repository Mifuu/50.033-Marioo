using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shotgun : MonoBehaviour
{
    [Header("Settings")]
    public GameObject projectile;
    public int ProjPerShot = 6;
    public float cooldown = 1.2f;
    private float cooldownTimer = 0.5f;
    public float scatterDeg = 30;
    public LayerMask hitMask;
    public float recoilSpeed = 4;
    public float knockSpeed = 4;
    public GameObject gunShotParticle;
    public GameObject gunHitParticle;

    [Space]
    public Transform barrelEnd;
    public Transform gunRotate;

    [Header("Damage")]
    public Damage damage;

    private float angle;
    private Vector2 cursor;
    public IntVariable score;


    [Header("Requirements")]
    public PlayerMovement playerMovement;

    void Update()
    {
        if (!Player.instance.Alive) return;

        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownTimer < 0)
        {
            Fire();
        }

        cursor = (Vector2)CursorPos.instance.worldPos;
        angle = Vector2.SignedAngle(Vector2.right, cursor - (Vector2)transform.position);

        gunRotate.rotation = Quaternion.Euler(Vector3.forward * angle);
        FlipSprite(Mathf.Abs(angle) > 90);
    }

    void Fire()
    {
        CameraShake.instance.Shake();

        playerMovement.Knock(-(cursor - (Vector2)transform.position) * recoilSpeed);
        cooldownTimer = cooldown;
        Instantiate(gunShotParticle, barrelEnd.position, Quaternion.identity);

        SFXManager.TryPlaySFX("proj_laser_2", Player.instance.gameObject);

        for (int i = 0; i < ProjPerShot; i++)
        {
            // scatter target pos
            float _scatterDeg = Random.Range(-scatterDeg / 2, scatterDeg);
            float _angle = angle + _scatterDeg;
            _angle *= Mathf.Deg2Rad;
            Vector2 target = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)).normalized;

            RaycastHit2D hit = Physics2D.Raycast(barrelEnd.position, target, 50f, hitMask);
            GameObject instance = Instantiate(projectile, barrelEnd.position, Quaternion.identity);
            LaserProjectile l = instance.GetComponent<LaserProjectile>();
            if (l == null)
            {
                return;
            }

            if (!hit)
            {
                l.target = cursor.normalized * 50f;
            }
            else
            {
                l.target = hit.point;

                var e = hit.transform.GetComponent<EnemyMovement>();
                if (e != null)
                {
                    e.Knock((cursor - (Vector2)transform.position) * knockSpeed);
                    // e.Dead();
                    GameManager.instance.AddScore();
                }
                var u = hit.transform.GetComponent<UoombaScript>();
                var d = hit.transform.GetComponent<Damageable>();
                if (d != null)
                {
                    d.TakeDamage(damage, target);
                    score.ApplyChange(1);
                }
                Instantiate(gunHitParticle, hit.point, Quaternion.identity);
            }
        }
    }

    void FlipSprite(bool val)
    {
        Vector3 scale = gunRotate.localScale;
        if ((val && Mathf.Sign(scale.y) == 1) || (!val && Mathf.Sign(scale.y) == -1))
            scale.y *= -1;
        gunRotate.localScale = scale;
    }
}
