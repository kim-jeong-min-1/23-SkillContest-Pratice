using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyGroup;
    [SerializeField] private List<Enemy> enemies;

    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        StartCoroutine(SetSpawner());

        while (!GameManager.Instance.isBoss)
        {
            var randEnemy = enemies[Random.Range(0, enemies.Count)];
            var randPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);

            Enemy enemy = Instantiate(randEnemy, randPos, Quaternion.identity, enemyGroup);
            enemy.HpMult(GameManager.Instance.StageNum);

            EnemySubject.Instance.AddEnemy(enemy);
            EnemySubject.Instance.enemyCount++;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    private IEnumerator SetSpawner()
    {
        float time = 0;

        while (!GameManager.Instance.isBoss)
        {
            time += Time.deltaTime;

            if (time >= 30f)
            {
                time = 0;
                minDelay *= 0.8f;
                maxDelay *= 0.8f;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
