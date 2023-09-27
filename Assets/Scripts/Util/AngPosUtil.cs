using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUtil
{
    public class AngPosUtil : MonoBehaviour
    {
        public static Vector2 GetAngularPos(float angle, float length, float randomAngle = 0)
        {
            Vector2 output;
            angle += Random.Range(-randomAngle / 2, randomAngle / 2);
            angle *= Mathf.Deg2Rad;

            output.x = length * Mathf.Cos(angle);
            output.y = length * Mathf.Sin(angle);

            return output;
        }

        public static float GetAngle(Vector2 p1, Vector2 p2)
        {
            return Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
        }
    }
}
