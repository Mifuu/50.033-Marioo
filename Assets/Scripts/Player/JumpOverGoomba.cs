using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpOverGoomba : MonoBehaviour
{
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask enemyMask;

    public Player player;
    public PlayerMovement playerMovement;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
    }

    void FixedUpdate()
    {
        JumpOnGoombaUpdate();
    }

    void JumpOnGoombaUpdate()
    {
        // check if alive
        if (!player.alive) return;

        // if player is falling down and the collider hit goomba then do dmg and add score

        if (playerMovement.rb.velocity.y >= 0)
        {
            return;
        }

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, -transform.up, maxDistance, enemyMask);
        foreach (var hit in hits)
        {
            if (hit)
            {
                EnemyMovement enemyMovement = hit.transform.GetComponent<EnemyMovement>();
                if (enemyMovement == null) continue;
                if (enemyMovement.isDead) continue;

                // player successfully jump on goomba
                Debug.Log("Jump on Goomba!");
                gm.AddScore();
                playerMovement.Jump(0.68f);

                enemyMovement.Dead();

            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}