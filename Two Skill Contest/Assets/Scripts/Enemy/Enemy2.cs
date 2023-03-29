using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    protected override IEnumerator EnemyAI_Update()
    {
        yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 12, 1.5f));

        int pCount = 0;
        int Count = 0;
        while (pCount != 2)
        {
            if(Count == 2)
            {
                Count = 0;
                pCount++;
                yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 12, 1.5f));
            }

            Count++;
            switch (Count)
            {
                case 1: 
                    yield return StartCoroutine(Pattern1());
                    break;
                case 2:
                    yield return StartCoroutine(Pattern2());
                    break;
            }
            yield return new WaitForSeconds(0.15f);
        }
        StartCoroutine(PatternEnd());
    }

    private IEnumerator Pattern1()
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i <= 360; i += 360 / 6)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, i, 0));
                bullet.ChangeColor(Color.red);
                bullet.speed = 35;
            }
            yield return new WaitForSeconds(0.1f);
        }        
    }

    private IEnumerator Pattern2()
    {
        StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15, 1.5f));
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 360; k+= 360 / 20)
            {
                var n1 = k / 2f;
                var n2 = 360f - i * (360 / 4f);
                var n3 = (int)n1 + n2;

                var bullet = InstantiateBullet(Quaternion.Euler(0, n3, 0));
                bullet.speed = 18f;
                bullet.ChangeColor(Color.red);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator PatternEnd()
    {
        int x;
        if (transform.position.x >= 0) x = 1;
        else x = -1;

        while (gameObject.activeSelf)
        {
            transform.Translate(new Vector3(x, 0, 1) * enemySpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
