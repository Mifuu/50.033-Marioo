using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General Settings")]
    private float offset; // current x offset between camera and target
    public float maxOffset; // max offset before camera starts moving
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the camera
    private float viewportHalfWidth;

    [Header("Requirements")]
    public Transform target; // Mario's Transform
    public Transform endLimit; // GameObject that indicates end of map
    public Transform startLimit;

    private Vector3 startPosition;

    void Start()
    {
        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        startX = startLimit.transform.position.x + viewportHalfWidth;
        endX = endLimit.transform.position.x - viewportHalfWidth;

        startPosition = transform.position;
    }

    void Update()
    {
        // updating offset value
        offset = this.transform.position.x - target.position.x;

        // check if need to move camera because target move outside offset bound
        float tarPosX = target.transform.position.x;
        Vector2 maxOffsetX = new Vector2(this.transform.position.x - maxOffset, this.transform.position.x + maxOffset);

        // if in the range, return, nothing to edit
        if (tarPosX > maxOffsetX.x && tarPosX < maxOffsetX.y) return;

        // out of max offset range need to move camera
        float desiredX = target.position.x;
        if (tarPosX <= maxOffsetX.x)    // if out to the left
            desiredX += maxOffset;
        else                            // if out to the right
            desiredX -= maxOffset;

        // check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }

    void Restart()
    {
        transform.position = startPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 maxOffsetMinPos = transform.position + Vector3.left * maxOffset;
        Vector3 maxOffsetMaxPos = transform.position + Vector3.right * maxOffset;

        Gizmos.DrawLine(startLimit.position + Vector3.up * 2, startLimit.position + Vector3.down * 2);
        Gizmos.DrawLine(endLimit.position + Vector3.up * 2, endLimit.position + Vector3.down * 2);
        Gizmos.DrawLine(maxOffsetMinPos + Vector3.up * 2, maxOffsetMinPos + Vector3.down * 1);
        Gizmos.DrawLine(maxOffsetMaxPos + Vector3.up * 2, maxOffsetMaxPos + Vector3.down * 1);
        Gizmos.DrawLine(endLimit.position, startLimit.position);
    }
}
