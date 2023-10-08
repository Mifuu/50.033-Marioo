using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonImageAnim : MonoBehaviour
{
    Image image;
    public float spd = 10;

    float yScale;
    float fac = 1;

    void Awake()
    {
        image = GetComponent<Image>();
        yScale = image.rectTransform.localScale.y;
    }

    void Update()
    {
        fac -= spd * Time.deltaTime;
        if (fac < -1) fac = 1;
        image.rectTransform.localScale = new Vector2(image.rectTransform.localScale.x, yScale * fac);
    }
}
