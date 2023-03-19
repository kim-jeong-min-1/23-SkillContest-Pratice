using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : Boss
{
    int prePattern = 0;
    int randPattern = 0;

    int repeatCount = 0;
    float patternTime = 0;
    float curTime = 0;

    Color[] spriteColors = new Color[2] { Color.red, Color.yellow };

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(BossAI_Update());
    }

    protected override IEnumerator BossAI_Update()
    {
        yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 45, 3.5f));

        while (!isDie)
        {
            do
            {
                randPattern = Random.Range(1, 4);
                yield return null;
            } while (prePattern == randPattern);

            prePattern = randPattern;

            switch (randPattern)
            {
                case 1:
                    yield return StartCoroutine(Pattern1());
                    break;
                case 2:
                    yield return StartCoroutine(Pattern2());
                    break;
                case 3:
                    yield return StartCoroutine(Pattern3());
                    break;
            }

            yield return new WaitForSeconds(1.8f);
        }       
    }

    private IEnumerator Pattern1()
    {
        repeatCount = 5;

        for (int i = 0; i < repeatCount; i++)
        {
            for (int j = 0; j < 360; j += Random.Range(10, 30))
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, j, 0));
                bullet.speed = 45;
                bullet.SetSpriteColor(spriteColors[0]);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield return null;
    }

    private IEnumerator Pattern2()
    {
        patternTime = 3.5f;
        curTime = 0;
        repeatCount = 5;
        int dir = 0;

        while (patternTime > curTime)
        {
            for (int i = 0; i < 360; i += 360 / repeatCount)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i + dir, 0));
                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));

                bullet1.speed = 40;  bullet2.speed = 40;
                bullet1.SetSpriteColor(spriteColors[0]); bullet2.SetSpriteColor(spriteColors[1]);
            }
            yield return new WaitForSeconds(0.08f);
            curTime += 0.08f;
            dir += 5;
        }
        yield return null;
    }

    private IEnumerator Pattern3()
    {
        repeatCount = 3;
        patternTime = 0.72f;
        curTime = 0;
        int dir = 0;

        StartCoroutine(MoveToPlayerPosition(Random.Range(-15, 15), 20f, 1f));

        for (int i = 0; i < repeatCount; i++)
        {
            while (patternTime > curTime)
            {
                for (int j = 0; j < 360; j += 360 / repeatCount)
                {
                    Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, j + dir, 0));
                    Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(j + dir), 0));

                    bullet1.speed = 40; bullet2.speed = 40;
                    bullet1.SetSpriteColor(spriteColors[0]); bullet2.SetSpriteColor(spriteColors[1]);
                }
                yield return new WaitForSeconds(0.08f);
                curTime += 0.08f;
                dir += 5;
            }
            curTime = 0;
            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
    }
}
