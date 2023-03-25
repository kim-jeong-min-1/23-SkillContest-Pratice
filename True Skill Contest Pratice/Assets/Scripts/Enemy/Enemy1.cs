using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override IEnumerator EnemyAI_Update()
    {
        StartCoroutine(Enemy1_Shooter());

        while (gameObject)
        {
            EnemyMovement();
            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator Enemy1_Shooter()
    {
        while (gameObject)
        {
            Bullet bullet = InstantiateBullet(Quaternion.Euler(0, 180f, 0));
            bullet.SetSpriteColor(Color.yellow);
            yield return new WaitForSeconds(shooter.cool);
        }
    }
}
