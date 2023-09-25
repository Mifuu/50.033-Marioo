using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorPixelSnapping : MonoBehaviour
{
    public int pixelSnapPerUnit = 16;

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Vector3 pos = transform.position;
            pos *= pixelSnapPerUnit;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            pos /= pixelSnapPerUnit;
            transform.position = pos;
        }
    }
}
