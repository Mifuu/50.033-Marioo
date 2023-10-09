using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShake : Singleton<CameraShake>
{
    public float shakeFrequency;
    private float magnitude;
    public float magnitudeDegradation = 0.95f;
    public UnityEvent onShake;

    Vector3 originalPos;

    protected override void Awake()
    {
        base.Awake();
        originalPos = transform.position;
    }

    void Update()
    {
        transform.position = originalPos + GetOffset();
    }

    void FixedUpdate()
    {
        magnitude *= magnitudeDegradation;
    }

    public void Shake(float mag = 0.2f)
    {
        magnitude = mag;
        onShake.Invoke();
    }

    Vector3 GetOffset()
    {
        float _mag = magnitude * Mathf.Sin(Time.time * shakeFrequency);
        Vector2 _out = Vector2.one * _mag;
        return new Vector3(_out.x, _out.y, originalPos.z);
    }
}
