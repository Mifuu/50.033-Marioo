using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointFollowObject : MonoBehaviour
{
    public Transform followTarget;
    private Vector3 followOffset;

    void Start()
    {
        if (!followTarget) return;
        followOffset = transform.position - followTarget.transform.position;
    }

    void Update()
    {
        if (!followTarget) return;
        transform.position = followTarget.transform.position + followOffset;
    }
}
