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
    public CanvasGroup hpCanvasGroup;
    protected float destroyDelay = 2;

    [Header("Damageable: Sprite Fade")]
    protected float spriteFadeTime = 2;
    public SpriteRenderer[] fadeSprites;

    [Header("Damageable: Drop")]
    public DropPool dropPool;

    [Header("Damageable: Particle")]
    public GameObject spawnParticle;
    public GameObject damageParticle;

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
            else if (hp == maxHP)
            {
                hpCanvasGroup.alpha = 0.5f;
            }
            else
            {
                hpCanvasGroup.alpha = 1;
                hpSlider.gameObject.SetActive(true);
                hpSlider.value = hp / (float)maxHP;
            }
        }
    }

    protected virtual void Awake()
    {
        HP = maxHP;

        if (spawnParticle != null)
        {
            Instantiate(spawnParticle, transform.position, Quaternion.identity);
        }
    }

    protected virtual void Dead(Damage dmg, Vector2 dir)
    {
        DropItem();
        StartCoroutine(FadeOutSpritesIE());
        StartCoroutine(DelayDestroy());
    }

    public virtual void TakeDamage(Damage dmg, Vector2 dir)
    {
        if (HP <= 0) return;

        HP -= dmg.value;

        if (damageParticle != null)
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
        }

        if (HP <= 0)
        {
            Dead(dmg, dir);
        }
    }

    private void DropItem()
    {
        DropPoolItem d = dropPool.GetDropPoolItem();

        if (d.drop != null)
        {
            Instantiate(d.drop, transform.position, Quaternion.identity);
        }

        SFXManager.TryPlaySFX(d.sfxName, gameObject);

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