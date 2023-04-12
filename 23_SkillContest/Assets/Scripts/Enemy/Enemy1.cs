using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override IEnumerator Enemy_AI()
    {
        StartCoroutine(Enemy1Shot());

        while (gameObject)
        {
            transform.Translate(new Vector3(0, 0, -enemySpeed * Time.deltaTime));
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Enemy1Shot()
    {
        while (gameObject)
        {
            Bullet bullet = InstantiateBullet(Quaternion.Euler(0, 180f, 0));
            bullet.SetColor(Color.yellow);
            bullet.speed = 24f;

            yield return new WaitForSeconds(0.6f);
        }
    }
}
