using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override IEnumerator EnemyAI_Update()
    {
        StartCoroutine(Enemy1_Shot());

        while (gameObject)
        {
            transform.Translate(Vector3.back * enemySpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Enemy1_Shot()
    {
        while (gameObject)
        {
            Bullet bullet = InstantiateBullet(Quaternion.Euler(0, 180, 0));
            bullet.ChangeColor(Color.yellow);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
