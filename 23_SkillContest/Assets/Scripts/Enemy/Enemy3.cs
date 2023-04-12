using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    protected override IEnumerator Enemy_AI()
    {

        yield return StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15f, 1.5f));

        while (!isDie)
        {
            yield return StartCoroutine(Pattern());
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Pattern()
    {
        StartCoroutine(MoveToPlayerPos(Random.Range(-10, 10), 15f, 1.5f));

        List<Bullet> bullets = new List<Bullet>();

        for (int p = 0; p < 2; p++)
        {
            for (int k = 0; k < 360; k += 360 / 12)
            {
                Bullet bullet = InstantiateBullet(Quaternion.Euler(0, k + p * 180f, 0));
                bullet.speed = 10f;
                bullets.Add(bullet);
                bullet.SetColor(Color.green);
            }
            yield return new WaitForSeconds(0.45f);

            for (int i = 0; i < bullets.Count; i++)
            {
                var dis = PlayerController.Instance.transform.position - bullets[i].transform.position;
                dis.y = 0;

                var rot = Quaternion.LookRotation(dis);
                bullets[i].transform.rotation = rot;
                bullets[i].speed = 25f;
            }
            bullets.Clear();
        }       
    }
}
