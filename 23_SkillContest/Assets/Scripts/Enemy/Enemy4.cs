using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{
    protected override IEnumerator Enemy_AI()
    {

        yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15f, 1.5f));

        yield return StartCoroutine(Pattern());
        StartCoroutine(PatterEnd());
    }

    private IEnumerator Pattern()
    {
        float dir1 = 270f;
        float dir2 = 90f;
        float curTime = 0;

        while (curTime < 5)
        {
            Bullet bullet1 = InstantiateBullet(Quaternion.Euler(0, dir1, 0));
            bullet1.SetColor(Color.blue);
            Bullet bullet2 = InstantiateBullet(Quaternion.Euler(0, dir2, 0));
            bullet2.SetColor(Color.blue);

            dir1 -= 5;
            dir2 += 5;
            curTime += 0.08f;
            yield return new WaitForSeconds(0.08f);
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
