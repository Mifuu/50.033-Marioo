using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : Singleton<BossEvent>
{
    public Boss boss;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        BossNextPhase();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            BossNextPhase();

        if (gameManager.GetEventProgress() > 0.25f && boss.phase == Boss.Phase.Invisible)
            BossNextPhase();
        else if (gameManager.GetEventProgress() > 0.5f && boss.phase == Boss.Phase.Silent)
            BossNextPhase();
        else if (gameManager.GetEventProgress() > 0.75f && boss.phase == Boss.Phase.Defence)
            BossNextPhase();
    }

    public void TryStartBoss()
    {
        if (boss.phase == Boss.Phase.None)
            BossNextPhase();
    }

    public void BossNextPhase()
    {
        switch (boss.phase)
        {
            case Boss.Phase.None:
                StartCoroutine(BossEventIE_Phase1());
                break;
            case Boss.Phase.Invisible:
                StartCoroutine(BossEventIE_Phase2());
                break;
            case Boss.Phase.Silent:
                StartCoroutine(BossEventIE_Phase3());
                break;
            case Boss.Phase.Defence:
                StartCoroutine(BossEventIE_Phase4());
                break;
        }
    }

    IEnumerator BossEventIE_Phase1()
    {
        // set boss position, phase, and material
        boss.StartPhase1();

        // heart beat loop

        yield return null;
    }

    IEnumerator BossEventIE_Phase2()
    {
        // set boss position, phase, and material
        boss.StartPhase2();



        yield return null;
    }

    IEnumerator BossEventIE_Phase3()
    {
        // set boss position, phase, and material
        boss.StartPhase3();



        yield return null;
    }

    IEnumerator BossEventIE_Phase4()
    {
        // set boss position, phase, and material
        boss.StartPhase4();



        yield return null;
    }

    IEnumerator HeartBeatEffectIE()
    {
        yield return null;
    }

}
