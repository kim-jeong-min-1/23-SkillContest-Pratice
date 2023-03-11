using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    private void Start()
    {
        StartCoroutine(EnemyAI_Update());
    }

    protected override IEnumerator EnemyAI_Update()
    {
        while (true)
        {
            transform.Translate(transform.forward * enemySpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }     
    }
}
