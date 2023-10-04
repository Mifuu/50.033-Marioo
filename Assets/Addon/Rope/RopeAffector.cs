using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAffector : MonoBehaviour
{
    public static List<RopeAffector> instances = new List<RopeAffector>();

    public float radius = 1;
    public float factor = 1;

    [HideInInspector] public Vector2 posNow;
    [HideInInspector] public Vector2 posOld;

    void FixedUpdate()
    {
        posOld = posNow;
        posNow = transform.position;
    }

    void OnEnable()
    {
        instances.Add(this);
    }

    void OnDisable()
    {
        instances.Remove(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Rope.GIZMOS_COLOR;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public static Vector2 GetForceAffector(Vector2 _pos)
    {
        Vector2 _force = Vector2.zero;
        foreach (RopeAffector _r in instances)
        {
            Vector2 deltaPos = _r.posNow - _pos;
            float sqrMag = Vector2.SqrMagnitude(deltaPos);

            if (sqrMag > _r.radius * _r.radius) continue;

            Vector2 _velocity = (_r.posNow - _r.posOld) * (1 / Time.fixedDeltaTime);
            _force += _velocity * _r.factor;
        }

        return _force;
    }
}
