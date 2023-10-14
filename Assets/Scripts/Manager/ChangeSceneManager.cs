using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : Singleton<ChangeSceneManager>
{
    public string nextSceneName;

    public void ChangeScene()
    {
        ChangeScene(nextSceneName);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

    public void ChangeSceneFade()
    {
        ChangeSceneFade(nextSceneName);
    }

    public void ChangeSceneFade(string scene)
    {
        ChangeSceneFade(scene, 0.5f);
    }

    public void ChangeSceneFade(string scene, float time)
    {
        Time.timeScale = 1;
        if (FadeUI.instance != null)
            StartCoroutine(ChangeSceneFadeIE(scene, time));
        else
            ChangeScene(scene);
    }

    IEnumerator ChangeSceneFadeIE(string scene, float time = 0.5f)
    {
        FadeUI.instance.FadeOut(time);
        yield return new WaitForSeconds(time);
        ChangeScene(scene);
    }
}
