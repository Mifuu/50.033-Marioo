using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/Damage", order = 4)]
public class Damage : ScriptableObject
{
    public int value;
    public float knockFac;
    public bool isHeavy;
}
