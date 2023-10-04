using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RopeEditorChild : MonoBehaviour
{
    public RopeEditorUpdater ropeEditorUpdater;

    private void OnDrawGizmosSelected()
    {
        ropeEditorUpdater?.OnDrawUpdate();
    }
}
