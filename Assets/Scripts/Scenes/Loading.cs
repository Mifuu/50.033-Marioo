using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loading : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float time;
    public Transform heli;

    public float heliTime = 5;

    public void BackCallback()
    {
        ChangeSceneManager.instance.ChangeSceneFade("MainMenu");
    }

    void Awake()
    {
        StartCoroutine(LoadingIE());
    }

    void Update()
    {
        time += Time.deltaTime;

        float ratio = time / heliTime;
        if (ratio > 1) ratio = 1;
        heli.transform.position = Vector3.Lerp(point1.position, point2.position, ratio);
    }

    IEnumerator LoadingIE()
    {
        yield return new WaitForSeconds(1.3f);
        ChangeSceneManager.instance.ChangeSceneFade("SampleScene 1", 1.2f);
    }
}
