using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(Rope))]
public class RopeEditorUpdater : MonoBehaviour
{
    public Rope rope;
    float timeSinceDrawSelected = 0;
    public float timeSinceDrawBeforeDisable = 2;

    int counter = 0;
    int updateEveryXUpdate = 3;

    void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }

    void EditorUpdate()
    {
        // slower update, to compensate for too fast update
        counter++;
        if (!(counter % updateEveryXUpdate == 0)) return;

        rope.EditorUpdate();
        timeSinceDrawSelected += Time.deltaTime;

        if (timeSinceDrawSelected > timeSinceDrawBeforeDisable)
        {
            enabled = false;
        }
    }

    public void OnDrawUpdate()
    {
        if (!rope.useInEditor)
        {
            enabled = false;
            return;
        }

        enabled = true;
        timeSinceDrawSelected = 0;
    }

    private void OnDrawGizmosSelected()
    {
        OnDrawUpdate();
    }
}
