using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Header("Damageable: Health")]
    public int maxHP = 20;
    public Slider hpSlider;
    protected float destroyDelay = 2;

    [Header("Damageable: Sprite Fade")]
    protected float spriteFadeTime = 2;
    public SpriteRenderer[] fadeSprites;

    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp <= 0) hp = 0;
            if (hp == 0)
            {   // don't show slider when hp is full
                hpSlider.gameObject.SetActive(false);
            }
            else
            {
                hpSlider.gameObject.SetActive(true);
                hpSlider.value = hp / (float)maxHP;
            }
        }
    }

    protected virtual void Awake()
    {
        HP = maxHP;
    }

    protected virtual void Dead(Damage dmg, Vector2 dir)
    {
        StartCoroutine(FadeOutSpritesIE());
        StartCoroutine(DelayDestroy());
    }

    public virtual void TakeDamage(Damage dmg, Vector2 dir)
    {
        HP -= dmg.value;

        if (HP <= 0)
        {
            Dead(dmg, dir);
        }
    }

    protected IEnumerator FadeOutSpritesIE()
    {
        float a = 1;
        while (a > 0)
        {
            foreach (var s in fadeSprites)
            {
                s.color = new Color(s.color.r, s.color.g, s.color.b, a);
            }
            a -= Time.deltaTime / spriteFadeTime;
            yield return null;
        }
    }

    protected IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}