using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Boss3 : Boss
{
    int prePattern = 0;
    int randPattern = 0;

    int repeatCount = 0;
    float patternTime = 0;
    float curTime = 0;

    private void Start()
    {
        bossPattern = StartCoroutine(BossAI_Update());
    }

    protected override IEnumerator BossAI_Update()
    {
        yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 45, 3.5f));

        while (!isDie)
        {
            do
            {
                randPattern = Random.Range(1, 7);
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
                case 4:
                    yield return StartCoroutine(Pattern4());
                    break;
                case 5:
                    yield return StartCoroutine(Pattern5());
                    break;
                case 6:
                    yield return StartCoroutine(Pattern6());
                    break;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator Pattern1()
    {
        repeatCount = 8;

        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-15, 15), Random.Range(35, 45), 0.5f));

            for (int k = 0; k < 40; k++)
            {
                Vector3 rot = new Vector3(0, (k / 40f) * 360 + (float)i / repeatCount * 360f, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.blue);
                bullet.speed = 40;
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }

    private IEnumerator Pattern2()
    {
        StartCoroutine(MoveToPlayerPosition(Random.Range(-20, 20), 45, 3f));
        repeatCount = 7;
        float cool = 0;
        float alpha = 0;

        for (int k = 0; k < (360 / repeatCount) * 2.5f; k++)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                Vector3 rot = new Vector3(0, ((float)i / repeatCount) * 360 + alpha * 360, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.blue);
                bullet.speed = 40;
            }
            cool += 0.05f;
            alpha = Mathf.Sin(cool);
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }

    private IEnumerator Pattern3()
    {
        StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 40f, 3f));
        repeatCount = 150;
        float cool = 0;
        float alpha = 0;

        for (int i = 0; i < repeatCount; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                Vector3 rot = new Vector3(0, i * 5f + (float)(k / 6f) * 360f * alpha, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.blue);
            }
            for (int k = 0; k < 6; k++)
            {
                Vector3 rot = new Vector3(0, -i * 5f + (float)(k / 6f) * 360f * alpha, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.white);
            }
            cool += 0.04f;
            alpha = Mathf.Sin(cool);
            yield return new WaitForSeconds(0.04f);
        }

        yield break;
    }
    private IEnumerator Pattern4()
    {
        repeatCount = 5;
        StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 35f, 3f));
        for (int i = 0; i < repeatCount; i++)
        {
            for (int j = 0; j < 360; j += Random.Range(10, 30))
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, j, 0));
                bullet.speed = 45;
                bullet.SetSpriteColor(Color.blue);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private IEnumerator Pattern5()
    {
        patternTime = 1.8f;
        curTime = 0;
        repeatCount = 5;
        int dir = 0;

        StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 35f, 3f));
        while (patternTime > curTime)
        {
            for (int i = 0; i < 360; i += 360 / repeatCount)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i + dir, 0));
                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));

                bullet1.speed = 40; bullet2.speed = 40;
                bullet1.SetSpriteColor(Color.blue); bullet2.SetSpriteColor(Color.white);
            }
            yield return new WaitForSeconds(0.08f);
            curTime += 0.08f;
            dir += 5;
        }
        yield return null;
    }

    private IEnumerator Pattern6()
    {
        repeatCount = 3;
        patternTime = 0.72f;
        curTime = 0;
        int dir = 0;

        StartCoroutine(MoveToPlayerPosition(Random.Range(-15, 15), 20f, 1.8f));

        for (int i = 0; i < repeatCount; i++)
        {
            while (patternTime > curTime)
            {
                for (int j = 0; j < 360; j += 360 / repeatCount)
                {
                    Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, j + dir, 0));
                    Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(j + dir), 0));

                    bullet1.speed = 40; bullet2.speed = 40;
                    bullet1.SetSpriteColor(Color.blue); bullet2.SetSpriteColor(Color.blue);
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
