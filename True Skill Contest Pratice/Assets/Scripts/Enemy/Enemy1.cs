using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(EnemyAI_Update());
    }

    protected override IEnumerator EnemyAI_Update()
    {
        StartCoroutine(DefaultEnemyShooter());

        while (gameObject)
        {
            EnemyMovement();
            yield return new WaitForEndOfFrame();
        }
    }
}
