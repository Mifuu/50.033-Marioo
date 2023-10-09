using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropPool", menuName = "ScriptableObjects/DropPool", order = 5)]
public class DropPool : ScriptableObject
{
    [SerializeField]
    private DropPoolItem[] dropPool;

    public DropPoolItem GetDropPoolItem()
    {
        // calculate weight
        int totalWeight = 0;
        foreach (var d in dropPool)
        {
            totalWeight += d.pool;
        }

        // int random weight
        int rand = Random.Range(1, totalWeight + 1);

        // check
        foreach (var d in dropPool)
        {
            rand -= d.pool;
            if (rand <= 0)
            {
                return d;
            }
        }

        return null;
    }
}

[System.Serializable]
public class DropPoolItem
{
    public GameObject drop;
    public int pool;
    public string sfxName;
}