using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : Singleton<FadeUI>
{
    public bool startFadeIn = true;

    public Image fadeImage;

    protected override void Awake()
    {
        base.Awake();
        if (startFadeIn) StartCoroutine(FadeInIE());
    }

    public void FadeOut(float time = 0.4f)
    {
        StartCoroutine(FadeOutIE(time));
    }

    IEnumerator FadeOutIE(float time = 0.4f)
    {
        float a = 0;

        while (a < 1)
        {
            a += (1 / time) * Time.deltaTime;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a);
            yield return null;
        }
    }

    public void FadeIn(float time = 0.4f)
    {
        StartCoroutine(FadeInIE(time));
    }

    IEnumerator FadeInIE(float time = 0.4f)
    {
        float a = 1;

        while (a > 0)
        {
            a -= (1 / time) * Time.deltaTime;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a);
            yield return null;
        }
    }
}
