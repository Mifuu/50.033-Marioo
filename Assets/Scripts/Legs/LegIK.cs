using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;
using System;

public class LegIK : MonoBehaviour
{
    public float length;

    [Header("Optional")]
    [SerializeField] private Transform endPoint;
    private Vector3 endPos;

    public bool altAngle = false;

    [Header("Requirement")]
    public Transform joint1;
    public Transform joint2;
    public Transform joint3;

    void Update()
    {
        UpdateJointPosition(altAngle);
    }

    void UpdateJointPosition(bool alternatePos = false)
    {
        // ensuring the length
        Vector3 _endPos = GetMaxEndPoint();
        Vector3 _startPos = joint1.transform.position;

        joint3.transform.position = _endPos;

        float x1 = _startPos.x;
        float y1 = _startPos.y;
        float x2 = _endPos.x;
        float y2 = _endPos.y;
        float r1 = length / 2;
        float r2 = length / 2;
        float d = Vector2.Distance(_startPos, _endPos);

        // ensuring d != length
        if (Mathf.Abs(d - length) < 0.001f) d = length - 0.1f;

        float x = 0;
        float y = 0;

        if (!alternatePos)
        {
            x = ((x1 + x2) / 2) + (r1 * r1 - r2 * r2) * (x2 - x1) / (2 * d * d) + Mathf.Sqrt(2 * ((r1 * r1 + r2 * r2) / (d * d)) - (Mathf.Pow((r1 * r1 - r2 * r2), 2) / Mathf.Pow(d, 4)) - 1) * (y2 - y1) / 2;
            y = ((y1 + y2) / 2) + (r1 * r1 - r2 * r2) * (y2 - y1) / (2 * d * d) + Mathf.Sqrt(2 * ((r1 * r1 + r2 * r2) / (d * d)) - (Mathf.Pow((r1 * r1 - r2 * r2), 2) / Mathf.Pow(d, 4)) - 1) * (x1 - x2) / 2;
        }
        else
        {
            x = ((x1 + x2) / 2) + (r1 * r1 - r2 * r2) * (x2 - x1) / (2 * d * d) - Mathf.Sqrt(2 * ((r1 * r1 + r2 * r2) / (d * d)) - (Mathf.Pow((r1 * r1 - r2 * r2), 2) / Mathf.Pow(d, 4)) - 1) * (y2 - y1) / 2;
            y = ((y1 + y2) / 2) + (r1 * r1 - r2 * r2) * (y2 - y1) / (2 * d * d) - Mathf.Sqrt(2 * ((r1 * r1 + r2 * r2) / (d * d)) - (Mathf.Pow((r1 * r1 - r2 * r2), 2) / Mathf.Pow(d, 4)) - 1) * (x1 - x2) / 2;
        }

        if (float.IsNaN(x))
        {
            x = joint2.transform.position.x;
            Debug.Log("X NAN");
        }
        if (float.IsNaN(y))
        {
            y = joint2.transform.position.y;
            Debug.Log("Y NAN");
        }

        joint2.transform.position = new Vector3(x, y, joint2.transform.position.z);

        joint1.transform.rotation = Quaternion.Euler(AngPosUtil.GetAngle(joint1.position, joint2.position) * Vector3.forward);
        joint2.transform.rotation = Quaternion.Euler(AngPosUtil.GetAngle(joint2.position, joint3.position) * Vector3.forward);
    }

    Vector3 GetMaxEndPoint()
    {
        Vector3 delta = GetEndPoint() - this.transform.position;
        if (delta.magnitude > length)
        {
            delta = delta.normalized * (length);
            return (Vector3)this.transform.position + delta;
        }
        return GetEndPoint();
    }

    public Vector3 GetEndPoint()
    {
        return ((endPoint != null) ? endPoint.transform.position : endPos);
    }

    public void SetEndPoint(Vector3 val)
    {
        if (endPoint == null)
            endPos = val;
        else
            endPoint.transform.position = val;
    }

    void OnDrawGizmos()
    {

    }
}
