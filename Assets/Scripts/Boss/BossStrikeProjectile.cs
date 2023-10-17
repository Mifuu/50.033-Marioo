using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class BossStrikeProjectile : GenericProjectile
{
    [Header("Strike Setting")]
    public float aimTime = 0.8f;
    public float fireDelay = 0.8f;

    public LayerMask aimMask;
    public LineRenderer lineRenderer;

    private Player player;

    protected override void Start()
    {
        player = Player.instance;
        StartCoroutine(AimIE());
    }

    IEnumerator AimIE()
    {
        while (time < aimTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 50f, aimMask);

            target = (player.transform.position - transform.position).normalized * 50;

            lineRenderer.SetPositions(new Vector3[] { transform.position, target });

            float angle = YUtil.AngPosUtil.GetAngle(player.transform.position, transform.position);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        yield return new WaitForSeconds(fireDelay);

        velocity = ((Vector2)target - (Vector2)transform.position).normalized * speed;

        target = (velocity.normalized * 50) + (Vector2)transform.position;
        while (true)
        {
            lineRenderer.SetPositions(new Vector3[] { transform.position, target });

            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsMaskMatched(collider.gameObject))
        {
            Player p = collider.gameObject.GetComponent<Player>();
            if (p)
            {
                p.TakeDamage();
            }
            if (collider.enabled) SFXManager.TryPlaySFX("strike_hit", gameObject);
            this.collider.enabled = false;
            this.lineRenderer.enabled = false;
            DestroyProjectile();
            StopAllCoroutines();
        }
    }
}
