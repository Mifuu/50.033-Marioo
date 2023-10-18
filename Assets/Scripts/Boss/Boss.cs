using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : Damageable
{

    public enum Phase { None = -1, Invisible = 0, Silent = 1, Defence = 2, Offence = 3 };
    [Space]
    public Phase phase;
    Coroutine currentPhase;

    [Header("Material Settings")]
    public Material bodyMat;
    public Material tentacleMat;

    [SerializeField]
    CamoMatSet[] materialSets;
    private Dictionary<Phase, CamoMatSet> phaseMats = new Dictionary<Phase, CamoMatSet>();

    [Header("Pos Ref")]
    public Transform setupPos;

    [Header("Requirements")]
    new public Collider2D collider;

    [Header("Attacks")]
    public GameObject strikeSpawnParticle;
    public GameObject strike1Obj;
    public Transform[] strike1Poses;
    public Transform[] strike2Poses;

    protected override void Awake()
    {
        base.Awake();
        foreach (var s in materialSets)
        {
            phaseMats.Add(s.phase, s);
        }
    }

    void Start()
    {
        if (hpCanvasGroup != null)
            hpCanvasGroup.alpha = 0.0f;
    }

    // invisible phase
    public void StartPhase1()
    {
        collider.enabled = false;
        phase = Phase.Invisible;

        CamoMatSet s = phaseMats[phase];
        SetMat(bodyMat, s.bodyMat, 0.1f);
        SetMat(tentacleMat, s.tentacleMat, 0.1f);
    }

    // silent phase
    public void StartPhase2()
    {
        collider.enabled = true;
        phase = Phase.Silent;

        MoveTo(setupPos.position, 0.0f);

        CamoMatSet s = phaseMats[phase];
        SetMat(bodyMat, s.bodyMat, 1f);
        SetMat(tentacleMat, s.tentacleMat, 1f);
    }

    // defence phase
    public void StartPhase3()
    {
        collider.enabled = true;
        phase = Phase.Defence;

        MoveTo(setupPos.position, 1);

        CamoMatSet s = phaseMats[phase];
        SetMat(bodyMat, s.bodyMat, 1f);
        SetMat(tentacleMat, s.tentacleMat, 1f);

        currentPhase = StartCoroutine(Phase4IE());
    }

    // offence phase
    public void StartPhase4()
    {
        SFXManager.TryPlaySFX("boss_growl", gameObject);

        collider.enabled = true;
        phase = Phase.Offence;

        MoveTo(setupPos.position, 1);

        CamoMatSet s = phaseMats[phase];
        SetMat(bodyMat, s.bodyMat, 1f);
        SetMat(tentacleMat, s.tentacleMat, 1f);

        StopCoroutine(currentPhase);
        currentPhase = StartCoroutine(Phase5IE());
    }

    IEnumerator Phase4IE()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Boss: Phase4IE");

        int lastAction = -1;

        while (true)
        {
            if (Random.value < 0.5f)
            {
                if (lastAction == 0) continue;
                lastAction = 0;
                yield return Phase4StrikeIE();
            }
            else
            {
                if (lastAction == 1) continue;
                lastAction = 1;
                yield return Phase4HorizontalIE();
            }
        }
    }

    IEnumerator Phase4StrikeIE()
    {
        Debug.Log("Boss: Phase4StrikeIE");

        foreach (var t in strike1Poses)
        {
            Instantiate(strike1Obj, t.position, Quaternion.identity);
            Instantiate(strikeSpawnParticle, t.position, Quaternion.identity);
            SFXManager.TryPlaySFX("strike_spawn", gameObject);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1);
    }

    IEnumerator Phase4HorizontalIE()
    {
        Debug.Log("Boss: Phase4HorizontalIE");
        yield return new WaitForSeconds(1);
    }

    IEnumerator Phase5IE()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Boss: Phase5IE");

        int lastAction = -1;

        while (true)
        {
            float rand = Random.value;
            if (rand < 0.33f)
            {
                if (lastAction == 0) continue;
                lastAction = 0;
                yield return Phase5StrikeIE();
            }
            else if (rand < 0.66f)
            {
                if (lastAction == 1) continue;
                lastAction = 1;
                yield return Phase5HorizontalIE();
            }
            else
            {
                if (lastAction == 2) continue;
                lastAction = 2;
                yield return Phase5BounceIE();
            }
        }
    }

    IEnumerator Phase5StrikeIE()
    {
        Debug.Log("Boss: Phase5StrikeIE");

        foreach (var t in strike2Poses)
        {
            Instantiate(strike1Obj, t.position, Quaternion.identity);
            Instantiate(strikeSpawnParticle, t.position, Quaternion.identity);
            SFXManager.TryPlaySFX("strike_spawn", gameObject);
            yield return new WaitForSeconds(0.6f);
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Phase5HorizontalIE()
    {
        Debug.Log("Boss: Phase5HorizontalIE");
        yield return new WaitForSeconds(0);
    }

    IEnumerator Phase5BounceIE()
    {
        Debug.Log("Boss: Phase5BounceIE");
        yield return new WaitForSeconds(0);
    }

    public override void TakeDamage(Damage dmg, Vector2 dir)
    {
        StartCoroutine(TakeDamageMatIE());

        SFXManager.TryPlaySFX("boss_damage", gameObject);

        base.TakeDamage(dmg, dir);
    }

    IEnumerator TakeDamageMatIE()
    {
        CamoMatSet s = phaseMats[phase];

        // set damage material
        SetMat(bodyMat, s.bodyMatDamaged, 0.2f);
        SetMat(tentacleMat, s.tentacleMatDamaged, 0.2f);

        yield return new WaitForSeconds(0.2f);

        // set damage material
        SetMat(bodyMat, s.bodyMat, 0.4f);
        SetMat(tentacleMat, s.tentacleMat, 0.4f);
    }

    public void MoveTo(Vector2 pos, float time)
    {
        transform.DOMove(pos, time)
        .SetEase(Ease.InOutCubic);
    }

    public void SetMat(Material mat, CamoMatSetting setting, float time)
    {
        mat.DOFloat(setting.alpha, "_Alpha", time);
        mat.DOFloat(setting.appearFac, "_AppearFac", time);
        mat.DOFloat(setting.glowFac, "_GlowFac", time);
    }

    public void MatBeatInvisible()
    {

    }

    [System.Serializable]
    public class CamoMatSet
    {
        public Phase phase;
        public CamoMatSetting bodyMat;
        public CamoMatSetting bodyMatDamaged;
        public CamoMatSetting tentacleMat;
        public CamoMatSetting tentacleMatDamaged;
    }

    [System.Serializable]
    public class CamoMatSetting
    {
        public float alpha;
        public float appearFac;
        public float glowFac;
    }
}
