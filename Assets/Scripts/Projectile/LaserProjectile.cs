using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : GenericProjectile
{
    [SerializeField]
    private LineRenderer lineRenderer;

    void Start()
    {
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2)CursorPos.instance.worldPos - (Vector2)transform.position);
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = target;
        lineRenderer.SetPositions(positions);

        StartCoroutine(Fired());
    }

    IEnumerator Fired()
    {
        yield return new WaitForSeconds(Random.Range(0.8f, 1.6f));
        Destroy(gameObject);
    }
}
