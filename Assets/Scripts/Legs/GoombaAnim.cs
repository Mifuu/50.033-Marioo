using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class GoombaAnim : MonoBehaviour
{
    public int numLeg = 4;
    public float rotationOffset;
    public float innerRadius = 1f;

    void FixedUpdate()
    {
        UpdateLegPlacement();
    }

    void UpdateLegPlacement()
    {

    }

    Vector2[] GetRayPositions()
    {
        Vector2[] output = new Vector2[numLeg];
        for (int i = 0; i < numLeg; i++)
        {
            output[i] = GetRayPosition(i);
        }

        return output;
    }

    Vector2 GetRayPosition(int legID)
    {
        Vector2 output;
        float angle = rotationOffset + legID * (360 / numLeg);
        output = AngPosUtil.GetAngularPos(angle, innerRadius);

        return output + (Vector2)transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2[] rayPositions = GetRayPositions();
        foreach (var pos in rayPositions)
        {
            Gizmos.DrawWireSphere(pos, 0.3f);
        }
    }
}
