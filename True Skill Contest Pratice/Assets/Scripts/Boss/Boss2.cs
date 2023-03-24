using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Boss2 : Boss
{
    int prePattern = 0;
    int randPattern = 0;

    int repeatCount = 0;
    float patternTime = 0;
    float curTime = 0;

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
                randPattern = Random.Range(3, 3);
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
        repeatCount = 8;

        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-15, 15), Random.Range(35, 45), 0.5f));

            for (int k = 0; k < 40; k++)
            {
                Vector3 rot = new Vector3(0, (k / 40f) * 360 + (float)i / repeatCount * 360f, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.green);
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

        for (int k = 0; k < (360 / repeatCount) * 5; k++)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                Vector3 rot = new Vector3(0, ((float)i / repeatCount) * 360 + alpha * 360, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.green);
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
        repeatCount = 40;
        //loat radius = 3;
        float dir = 0;
        Vector3 center = transform.position;

        //for (int j = 0; j < repeatCount; j++)
        //{
        //    for (int i = 0; i < 360; i += 360 / 12)
        //    {
        //        var pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(i * Mathf.Deg2Rad) * radius) + center;
        //        Bullet bullet = InstantiateBullet(Quaternion.Euler(0, i, 0));
        //        bullet.transform.position = pos;
        //        bullet.isStop = true;
        //        bullet.SetSpriteColor(Color.green);
        //    }
        //    radius += 3;
        //    yield return new WaitForSeconds(0.02f);
        //}

        repeatCount = 7;
        for (int i = 0; i < (360 / repeatCount) * 5; i++)
        {
            for (int j = 0; j < 360; j += 360 / repeatCount)
            {
                var rot = j + dir;
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, rot, 0));
                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -rot, 0));
                bullet1.SetSpriteColor(Color.green);
                bullet2.SetSpriteColor(Color.green);
                bullet1.speed = 40f;
                bullet2.speed = 40f;
            }
            dir += 2f;
            yield return new WaitForSeconds(0.15f);
        }


        yield break;
    }
}
