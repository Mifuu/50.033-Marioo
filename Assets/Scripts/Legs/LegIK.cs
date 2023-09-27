using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;
using System;

public class LegIK : MonoBehaviour
{
    public float length;

    public Vector2 endPoint;

    public bool altAngle = false;

    [Header("Requirement")]
    public Transform joint1;
    public Transform joint2;
    public Transform joint3;

    void FixedUpdate()
    {
        UpdateJointPosition(altAngle);
    }

    void UpdateJointPosition(bool alternatePos = false)
    {
        // ensuring the length
        Vector3 _end = GetMaxEndPoint();

        joint1.transform.position = this.transform.position;
        joint3.transform.position = _end;

        float x1 = this.transform.position.x;
        float y1 = this.transform.position.y;
        float x2 = _end.x;
        float y2 = _end.y;
        float r1 = length / 2;
        float r2 = length / 2;
        float d = Vector2.Distance(this.transform.position, _end);

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

        joint2.transform.position = new Vector3(x, y, joint2.transform.position.z);

        joint1.transform.rotation = Quaternion.Euler(AngPosUtil.GetAngle(joint1.position, joint2.position) * Vector3.forward);
        joint2.transform.rotation = Quaternion.Euler(AngPosUtil.GetAngle(joint2.position, joint3.position) * Vector3.forward);
    }

    Vector3 GetMaxEndPoint()
    {
        Vector3 delta = (Vector3)endPoint - this.transform.position;
        if (delta.magnitude > length)
        {
            delta = delta.normalized * (length);
            return this.transform.position + delta;
        }
        return endPoint;
    }

    void OnDrawGizmos()
    {

    }
}
