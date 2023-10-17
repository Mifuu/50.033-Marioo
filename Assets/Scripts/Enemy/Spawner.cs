using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public static List<SpawnerScript> spawnerScripts = new List<SpawnerScript>();

    public Vector2 size;
    public GameObject spawn;
    public Vector2 periodRange;
    public Vector2Int numRange;
    public bool spawnOnStart = true;
    public Vector2 timeRange = new Vector2(0, 280);

    void OnEnable() => spawnerScripts.Add(this);
    void OnDisable() => spawnerScripts.Remove(this);

    void Start()
    {
        StartCoroutine(DelaySpawn());
        StartCoroutine(DelayStop());
    }

    IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(timeRange.y);
        StopAllCoroutines();
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(timeRange.x);
        StartCoroutine(SpawnIE());
    }

    IEnumerator SpawnIE()
    {
        if (spawnOnStart) Spawn();

        float period = Random.Range(periodRange.x, periodRange.y);
        yield return new WaitForSeconds(period);

        if (!spawnOnStart) Spawn();

        StartCoroutine(SpawnIE());
    }

    void Spawn()
    {
        float num = Random.Range(numRange.x, numRange.y + 1);
        for (int i = 0; i < num; i++)
        {
            float x = Random.Range(transform.position.x - size.x / 2, transform.position.x + size.x / 2);
            float y = Random.Range(transform.position.y - size.y / 2, transform.position.y + size.y / 2);
            Instantiate(spawn, new Vector2(x, y), Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
