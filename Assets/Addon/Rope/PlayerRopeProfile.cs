using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeProfile : MonoBehaviour
{
    public static PlayerRopeProfile instance;

    [Header("Settings")]
    public Mode mode = Mode.Box;
    public enum Mode { Box, Radius }
    public Vector2 extents = new Vector2(25, 15);
    public float radius = 40;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Rope.GIZMOS_COLOR;
        if (mode == Mode.Box)
        {
            Gizmos.DrawWireCube(transform.position, extents * 2);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    public bool IsInArea(Vector2 point)
    {
        if (mode == Mode.Box)
        {
            Vector2 center = transform.position;
            if (
                point.x < center.x + extents.x && point.x > center.x - extents.x &&
                point.y < center.y + extents.y && point.y > center.y - extents.y
            )
                return true;

            return false;
        }
        else
        {
            float dstSqr = ((Vector2)transform.position - point).sqrMagnitude;
            float radiusSqr = Mathf.Pow(radius, 2);

            if (dstSqr > radiusSqr)
                return false;

            return true;
        }
    }
}
