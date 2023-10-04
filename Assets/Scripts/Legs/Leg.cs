using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

[RequireComponent(typeof(LegIK))]
public class Leg : MonoBehaviour
{
    [Header("Contact Settings")]
    public float length;
    public float rayLength;
    public float angle;
    public float angleDelta;
    public LayerMask layerMask;
    public float standbyLength;

    [Header("Max Settings")]
    public Vector2 maxXDist;
    public float maxDist;

    [Header("Requirements")]
    public LegIK legIK;

    public bool contact = false;
    public Vector2 endPointTarget;
    private Vector3 endPointVelocity;

    [Header("EndPoint Settings")]
    public float contactSmoothTime = 0.1f;
    public float uncontactSmoothTime = 0.4f;

    [Header("Sprite Settings")]
    public bool altIK;

    [Header("Tracking Velocity")]
    private Vector2 oldPos;
    private Vector2 pos;
    private Vector2 velocity;

    // [Header("SFX")] // https://www.zapsplat.com/sound-effect-category/footsteps/
    private string[] stepSFXs = { "mon_step_1", "mon_step_2", "mon_step_3", "mon_step_4" };

    void Awake()
    {
        pos = transform.position;
        oldPos = pos;
    }

    void FixedUpdate()
    {
        UpdatePos();
        UpdateContact();
        UpdateLegPos();
    }

    void UpdateContact()
    {
        if (contact)
        {
            // Debug.Log(angle + ", " + AngPosUtil.GetAngle(transform.position, legIK.endPoint));

            // if dist x more than max
            if (legIK.GetEndPoint().x > transform.position.x + maxXDist.y)
            {
                contact = false;
            }
            else if (legIK.GetEndPoint().x < transform.position.x - maxXDist.x)
            {
                contact = false;
            }

            if (Vector2.Distance(legIK.GetEndPoint(), transform.position) > maxDist)
            {
                contact = false;
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


                SFXManager.TryPlaySFX(stepSFXs, this.gameObject);

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

        // legIK.SetEndPoint(endPointTarget);
        legIK.SetEndPoint(Vector3.SmoothDamp(legIK.GetEndPoint(), endPointTarget, ref endPointVelocity, contact ? contactSmoothTime : uncontactSmoothTime));
        // legIK.endPoint = endPointTarget;
    }

    void UpdatePos()
    {
        oldPos = pos;
        pos = transform.position;
        velocity = (pos - oldPos) / Time.deltaTime;
    }

    Vector2? TryGetContactPoint()
    {
        float rayAngle = angle;

        float angleLeft = angle - angleDelta;
        float angleRight = angle + angleDelta;

        if (Mathf.Abs(angleLeft) - Mathf.Abs(angleRight) < 0)
        {
            float _tmp = angleLeft;
            angleLeft = angleRight;
            angleRight = _tmp;
        }

        // differ ray direction based on velocity
        if (Mathf.Abs(velocity.x) < 0.001f)
        {
            rayAngle = angle;
            Debug.Log("0");
        }
        else if (velocity.x > 0)
        {
            rayAngle = angleRight;
            Debug.Log("1");
        }
        else
        {
            rayAngle = angleLeft;
            Debug.Log("-1");
        }

        Vector2 rayDir = AngPosUtil.GetAngularPos(rayAngle, rayLength);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, rayLength, layerMask);

        if (hit)
        {
            return hit.point;
        }

        return null;
    }

    void OnDrawGizmosSelected()
    {
        // draw init ray
        Gizmos.color = Color.yellow;
        Vector2 targetInitPos = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle, rayLength);
        Vector2 targetInitPos1 = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle - angleDelta, rayLength);
        Vector2 targetInitPos2 = (Vector2)transform.position + AngPosUtil.GetAngularPos(angle + angleDelta, rayLength);
        Gizmos.DrawWireSphere(targetInitPos, 0.2f);
        Gizmos.DrawLine(transform.position, targetInitPos);
        Gizmos.DrawLine(transform.position, targetInitPos1);
        Gizmos.DrawLine(transform.position, targetInitPos2);

        // draw max ray
        Gizmos.color = new Color(1, 0.5f, 0);
        float y = transform.position.y;
        Vector2 p1 = new Vector2(transform.position.x - maxXDist.x, y);
        Vector2 p2 = new Vector2(transform.position.x + maxXDist.y, y);
        Gizmos.DrawLine(p1, p2);
    }

    void OnValidate()
    {
        legIK.length = length;
        legIK.altAngle = altIK;
    }
}
