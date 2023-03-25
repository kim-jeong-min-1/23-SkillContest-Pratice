using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Boss2 : Boss
{
    int prePattern = 0;
    int randPattern = 0;
    int repeatCount = 0;

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
        StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 40f, 3f));
        repeatCount = 300;
        float cool = 0;
        float alpha = 0;

        for (int i = 0; i < repeatCount; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                Vector3 rot = new Vector3(0, i * 5f + (float)(k / 6f) * 360f * alpha, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.yellow);
            }
            for (int k = 0; k < 6; k++)
            {
                Vector3 rot = new Vector3(0, -i * 5f + (float)(k / 6f) * 360f * alpha, 0);
                var bullet = InstantiateBullet(Quaternion.Euler(rot));
                bullet.SetSpriteColor(Color.green);
            }
            cool += 0.04f;
            alpha = Mathf.Sin(cool);
            yield return new WaitForSeconds(0.04f);
        }
        yield break;
    }
}
