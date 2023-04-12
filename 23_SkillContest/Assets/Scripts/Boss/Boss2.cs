using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Boss
{
    protected override IEnumerator Boss_AI()
    {
        int randPattern = 0;
        int tempPattern = 0;

        yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10f, 10f), 18f, 3f));
        while (!isDie)
        {
            do
            {
                randPattern = Random.Range(1, 4);
                yield return null;
            } while (randPattern == tempPattern);

            tempPattern = randPattern;

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
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Pattern1()
    {
        for (int i = 0; i < 8; i++)
        {
            StartCoroutine(MoveToPlayerPos(Random.Range(-10f, 10f), Random.Range(15f, 18f), 0.8f));
            for (int k = 0; k < 360; k += 360 / 30)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k + i * (360 / 30f), 0));
                bullet.SetColor(Color.green);
                bullet.speed = 25f;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator Pattern2()
    {
        float time = 0;
        float dir = 0;

        for (int i = 0; i < (360 / 6) * 4; i++)
        {
            for (int k = 0; k < 360; k+= 360 / 6)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k + dir * 360, 0));
                bullet.SetColor(Color.green);
            }
            time += 0.05f;
            dir = Mathf.Sin(time);
            yield return new WaitForSeconds(0.05f);
        }

    }

    private IEnumerator Pattern3()
    {
        float time = 0;
        float dir = 0;

        for (int i = 0; i < 150; i++)
        {
            for (int k = 0; k < 360; k+= 360 / 6)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i * 5f + dir * k, 0));
                bullet1.SetColor(Color.green);

                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -i * 5f + dir * k, 0));
                bullet2.SetColor(Color.yellow);
            }
            time += 0.05f;
            dir = Mathf.Sin(time);
            yield return new WaitForSeconds(0.05f);
        }
    }
}