using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    public float rotation;
    private Quaternion quaternion;

    void Awake()
    {
        quaternion = Quaternion.Euler(0, 0, rotation);
    }

    void Update()
    {
        transform.rotation = quaternion;
    }
}
