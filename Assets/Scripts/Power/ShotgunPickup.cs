using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    public GameObject particle;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player == null) return;

        player.EnableShotgun(true);

        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
