using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override IEnumerator EnemyAI_Update()
    {
        while (gameObject != null)
        {
            transform.Translate(transform.forward* enemySpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
