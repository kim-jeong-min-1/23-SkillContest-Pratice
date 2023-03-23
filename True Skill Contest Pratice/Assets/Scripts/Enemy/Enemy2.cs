using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    int repeatCount = 0;
    Color spriteColor = Color.red;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(EnemyAI_Update());
    }

    protected override IEnumerator EnemyAI_Update()
    {
        yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 33, 1.25f));

        int patternRepeatCount = 0;
        int count = 0;

        while (patternRepeatCount != 2)
        {
            count++;

            switch (count)
            {
                case 1:
                    yield return StartCoroutine(pattern1());
                    break;
                case 2:
                    yield return StartCoroutine(pattern2());
                    break;
            }

            if (count == 2)
            {
                count = 0;
                patternRepeatCount++;
                yield return StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 35, 1.25f));
            }
            yield return null;
        }
        StartCoroutine(patternEnd());
    }

    private IEnumerator pattern1()
    {
        repeatCount = 5;
        for (int i = 0; i < repeatCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / 6)
            {
                var bullet = InstantiateBullet(Quaternion.Euler(0, j, 0));
                bullet.speed = 90;
                bullet.SetSpriteColor(spriteColor);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    private IEnumerator pattern2()
    {
        StartCoroutine(MoveToPlayerPosition(Random.Range(-30, 30), 33, 1.5f));

        repeatCount = 4;
        for (int i = 0; i < repeatCount; i++)
        {
            for (int k = 0; k < 360; k += 360 / 20)
            {
                var n1 = k / 2f;
                var n2 = 360f - i * (360 / repeatCount);
                var n3 = (int)(n1 + n2);

                var bullet = InstantiateBullet(Quaternion.Euler(new Vector3(0, n3, 0)));
                bullet.speed = 40;
                bullet.SetSpriteColor(spriteColor);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    private IEnumerator patternEnd()
    {
        int x;
        if (transform.position.x >= 0) x = 1;
        else x = -1;

        while (gameObject.activeSelf)
        {
            var speed = Time.deltaTime * enemySpeed;
            transform.Translate(new Vector3(x, 0, 1) * speed);

            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
