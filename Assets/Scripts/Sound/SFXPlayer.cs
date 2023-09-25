using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void Play(string name)
    {
        SFXManager.instance.PlaySFX(name, this.gameObject);
    }
}
