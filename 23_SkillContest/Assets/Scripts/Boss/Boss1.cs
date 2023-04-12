using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Boss
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
            for (int k = 0; k < 360; k += 360 / Random.Range(10, 30))
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k, 0));
                bullet.SetColor(Color.red);
                bullet.speed = 25f;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Pattern2()
    {
        float cur = 0;
        float dir = 0;

        StartCoroutine(MoveToPlayerPos(Random.Range(-10f, 10f), 18f, 1.5f));
        while (cur < 5f)
        {
            cur += Time.deltaTime;
            for (int i = 0; i < 360; i+= 360 / 5)
            {
                Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, i + dir, 0));
                bullet1.SetColor(Color.red);
                bullet1.speed = 15f;

                Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));
                bullet2.SetColor(Color.yellow);
                bullet2.speed = 15f;
            }
            dir += 5;
            cur += 0.08f;
            yield return new WaitForSeconds(0.08f);
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
                    bullet1.SetColor(Color.red);
                    bullet1.speed = 18f;

                    Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, -(i + dir), 0));
                    bullet2.SetColor(Color.yellow);
                    bullet2.speed = 18f;
                }
                dir += 5;
                cur += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
