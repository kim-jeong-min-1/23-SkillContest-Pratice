using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Boss
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
                randPattern = Random.Range(1, 7);
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
            yield return new WaitForSeconds(0.35f);
        }
    }

    private IEnumerator Pattern1()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 360; k += 360 / Random.Range(10, 30))
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k, 0));
                bullet.SetColor(Color.white);
                bullet.speed = 30f;
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator Pattern2()
    {
        float cur = 0;
        float dir = 0;

        StartCoroutine(MoveToPlayerPos(Random.Range(-10f, 10f), 18f, 1.5f));
        while (cur < 3f)
        {
            cur += Time.deltaTime;
            for (int i = 0; i < 360; i += 360 / 5)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i + dir, 0));
                bullet1.SetColor(Color.blue);
                bullet1.speed = 20f;

                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));
                bullet2.SetColor(Color.white);
                bullet2.speed = 20f;
            }
            dir += 5;
            cur += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator Pattern3()
    {
        StartCoroutine(MoveToPlayerPos(Random.Range(-8f, 8f), 12f, 1.5f));
        float cur = 0;
        float dir = 0;

        for (int k = 0; k < 3; k++)
        {
            while (cur < 0.75f)
            {
                cur += Time.deltaTime;
                for (int i = 0; i < 360; i += 360 / 5)
                {
                    Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i + dir, 0));
                    bullet1.SetColor(Color.blue);
                    bullet1.speed = 15f;

                    Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));
                    bullet2.SetColor(Color.white);
                    bullet2.speed = 15f;
                }
                dir += 5;
                cur += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator Pattern4()
    {
        for (int i = 0; i < 8; i++)
        {
            StartCoroutine(MoveToPlayerPos(Random.Range(-12f, 12f), Random.Range(15f, 18f), 0.8f));
            for (int k = 0; k < 360; k += 360 / 36)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k + i * (360 / 30f), 0));
                bullet.SetColor(Color.blue);
                bullet.speed = 25f;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Pattern5()
    {
        float time = 0;
        float dir = 0;

        for (int i = 0; i < (360 / 6); i++)
        {
            for (int k = 0; k < 360; k += 360 / 6)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k + dir * 360, 0));
                bullet.SetColor(Color.blue);
            }
            time += 0.05f;
            dir = Mathf.Sin(time);
            yield return new WaitForSeconds(0.05f);
        }

    }

    private IEnumerator Pattern6()
    {
        float time = 0;
        float dir = 0;

        for (int i = 0; i < 60; i++)
        {
            for (int k = 0; k < 360; k += 360 / 6)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i * 5f + dir * k, 0));
                bullet1.SetColor(Color.blue);

                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -i * 5f + dir * k, 0));
                bullet2.SetColor(Color.white);
            }
            time += 0.05f;
            dir = Mathf.Sin(time);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
