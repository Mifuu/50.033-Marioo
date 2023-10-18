using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinPickup : MonoBehaviour
{
    public int value = 5;
    public GameObject particle;

    public LayerMask groundMask;
    private float velocityY;
    public float gravity = 2;

    new BoxCollider2D collider;
    float extentY;

    public string sfxName;

    public IntVariable coin;
    public IntVariable score;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        extentY = collider.bounds.extents.y;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player == null) return;

        coin.ApplyChange(value);
        score.ApplyChange(value);

        Instantiate(particle, transform.position, Quaternion.identity);
        SFXManager.TryPlaySFX(sfxName, player.gameObject);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        velocityY -= gravity * Time.fixedDeltaTime;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down * extentY, Vector2.down, velocityY, groundMask);
        if (hit)
        {
            transform.position = hit.point + Vector2.up * extentY;
        }
        else
        {
            transform.Translate(Vector2.down * velocityY);
        }
    }
}
