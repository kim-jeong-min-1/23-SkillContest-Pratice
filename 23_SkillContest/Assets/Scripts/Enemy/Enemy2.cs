using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    protected override IEnumerator Enemy_AI()
    {
        int count = 0;
        int patternCount = 0;

        yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15f, 1.5f));

        while (patternCount != 2)
        {
            count++;
            switch (count)
            {
                case 1:
                    yield return StartCoroutine(Patter1());
                    break;
                case 2:
                    yield return StartCoroutine(Patter2());
                    break;
            }

            if (count == 2)
            {
                count = 0;
                patternCount++;
            }
            yield return new WaitForSeconds(0.25f);
        }
        StartCoroutine(PatterEnd());
    }

    private IEnumerator Patter1()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 360; k += 360 / 6)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k, 0));
                bullet.speed = 25f;
                bullet.SetColor(Color.red);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Patter2()
    {
        StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15f, 1.5f));
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 360; k+= 360 / 20)
            {
                var n1 = k / 2f;
                var n2 = i * (360 / 4);
                var n3 = (int)n1 + n2;

                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, n3, 0));
                bullet.speed = 15f;
                bullet.SetColor(Color.red);
            }
            yield return new WaitForSeconds(0.1f);
        }    
    }

    private IEnumerator PatterEnd()
    {
        int x;
        if (transform.position.x >= 0) x = 1;
        else x = -1;

        while (gameObject)
        {
            var speed = enemySpeed * Time.deltaTime;

            transform.Translate(new Vector3(x * speed, 0, speed));

            yield return new WaitForFixedUpdate();
        }
    }
}
