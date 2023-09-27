using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

[RequireComponent(typeof(LegIK))]
public class Leg : MonoBehaviour
{
    [Header("Contact Settings")]
    public float length;
    public float angle;
    public float randomAngle;
    public LayerMask layerMask;
    public int maxPlacementTrial = 15;
    public float standbyLength;

    [Header("Max Settings")]
    public float maxLength;
    public float maxDeltaAngle;
    public float maxAngleOffset;

    [Header("Requirements")]
    public LegIK legIK;

    [HideInInspector] public bool contact = false;
    private Vector2 endPointTarget;
    private Vector3 endPointVelocity;

    [Header("EndPoint Settings")]
    public float contactSmoothTime = 0.1f;
    public float uncontactSmoothTime = 0.4f;

    [Header("Sprite Settings")]
    public bool altIK;

    void FixedUpdate()
    {
        UpdateContact();
        UpdateLegPos();
    }

    void UpdateContact()
    {
        if (contact)
        {
            // Debug.Log(angle + ", " + AngPosUtil.GetAngle(transform.position, legIK.endPoint));

            // if dist or angle is more than limit, contact = false
            if (Vector2.Distance(legIK.endPoint, transform.position) > maxLength)
            {
                contact = false;

                // move the end point to standby position
            }
            else if (Mathf.Abs((angle + maxAngleOffset) - AngPosUtil.GetAngle(transform.position, legIK.endPoint)) > maxDeltaAngle)
            {
                contact = false;

                // move the end point to standby position
            }
        }
        else
        {
            // if can establish new contact, contact = true
            Vector2? contactPoint = TryGetContactPoint();

            if (contactPoint == null)
            {
                // move the end point to standby position
                contact = false;
                return;
            }
            else
            {
                // move the endPoint to the contact point
                Vector2 _contactPoint = (Vector2)contactPoint;
                endPointTarget = new Vector2(_contactPoint.x, _contactPoint.y);

                contact = true;
            }
        }
    }

    void UpdateLegPos()
    {
        if (!contact)
        {
            endPointTarget = AngPosUtil.GetAngularPos(angle, standbyLength) + (Vector2)transform.position;
        }


        legIK.endPoint = Vector3.SmoothDamp(legIK.endPoint, endPointTarget, ref endPointVelocity, contact ? contactSmoothTime : uncontactSmoothTime);
        // legIK.endPoint = endPointTarget;
    }

    Vector2? TryGetContactPoint()
    {
        for (int i = 0; i < maxPlacementTrial; i++)
        {
            Vector2 rayDir = AngPosUtil.GetAngularPos(angle, length, randomAngle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, length, layerMask);

            if (hit)
            {
                return hit.point;
            }
        }

        return null;
    }

    void OnDrawGizmos()
    {
        // draw init ray
        Gizmos.color = Color.yellow;
        Vector2 targetInitPos = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle, length);
        Vector2 targetInitPosRand1 = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle - randomAngle / 2, length);
        Vector2 targetInitPosRand2 = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle + randomAngle / 2, length);
        Gizmos.DrawWireSphere(targetInitPos, 0.2f);
        Gizmos.DrawLine(transform.position, targetInitPos);
        Gizmos.DrawLine(transform.position, targetInitPosRand1);
        Gizmos.DrawLine(transform.position, targetInitPosRand2);

        // draw max ray
        Gizmos.color = new Color(1, 0.5f, 0);
        Vector2 max1 = (Vector2)transform.position + AngPosUtil.GetAngularPos((angle + maxAngleOffset) - maxDeltaAngle, maxLength);
        Vector2 max2 = (Vector2)transform.position + AngPosUtil.GetAngularPos((angle + maxAngleOffset) + maxDeltaAngle, maxLength);
        Gizmos.DrawLine(transform.position, max1);
        Gizmos.DrawLine(transform.position, max2);
    }

    void OnValidate()
    {
        legIK.length = length;
        legIK.altAngle = altIK;
    }
}
