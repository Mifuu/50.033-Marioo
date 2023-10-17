using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class GenericGun : MonoBehaviour
{
    [Header("Settings")]
    public GameObject projectile;
    public float cooldown = 1.2f;
    private float cooldownTimer = 0.5f;
    public int projPerShot = 1;
    public float timePerProjectile = 0.1f;
    public float scatterDeg = 0;
    public LayerMask hitMask;
    public float recoilSpeed = 4;
    public float knockSpeed = 4;

    [Header("Effects")]
    public GameObject gunShotParticle;
    public string fireSFX;
    public float screenShakeFac = 1;

    [Space]
    public Transform barrelEnd;
    public Transform gunRotate;

    [Header("Damage")]
    public Damage damage;

    private float angle;
    private Vector2 cursor;

    [Header("Requirements")]
    public PlayerMovement playerMovement;

    void Update()
    {
        if (!Player.instance.Alive) return;

        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownTimer < 0)
        {
            StartCoroutine(FireIE());
        }

        cursor = (Vector2)CursorPos.instance.transform.position;
        angle = Vector2.SignedAngle(Vector2.right, cursor - (Vector2)transform.position);
        // angle = YUtil.AngPosUtil.GetAngle(cursor, transform.position);

        gunRotate.rotation = Quaternion.Euler(Vector3.forward * angle);
        FlipSprite(Mathf.Abs(angle) > 90);
    }

    IEnumerator FireIE()
    {
        CameraShake.instance.Shake(screenShakeFac);

        cooldownTimer = cooldown;

        for (int i = 0; i < projPerShot; i++)
        {
            // scatter target pos
            // float _scatterDeg = Random.Range(-scatterDeg / 2, scatterDeg);
            // float _angle = angle + _scatterDeg;
            // _angle *= Mathf.Deg2Rad;
            // Vector2 target = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)).normalized;

            GameObject instance = Instantiate(projectile, barrelEnd.position, Quaternion.identity);
            GenericProjectile l = instance.GetComponent<GenericProjectile>();
            if (l == null)
            {
                yield break;
            }

            l.target = YUtil.AngPosUtil.GetAngularPos(angle, 50, scatterDeg) * 50f;
            l.damage = damage;

            playerMovement.Knock(-(cursor - (Vector2)transform.position) * recoilSpeed);
            Instantiate(gunShotParticle, barrelEnd.position, Quaternion.Euler(transform.rotation.z * Vector3.forward));
            SFXManager.TryPlaySFX(fireSFX, Player.instance.gameObject);

            if (timePerProjectile != 0)
            {
                yield return new WaitForSeconds(timePerProjectile);
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
