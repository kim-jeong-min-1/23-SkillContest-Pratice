using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private Transform enemyGroup;
    [SerializeField] private List<Enemy> enemies;

    private float curTime = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        StartCoroutine(SetSpawnDealy());

        while (true)
        {
            var randEnemy = enemies[Random.Range(0, enemies.Count)];
            var randPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);

            Enemy enemy = Instantiate(randEnemy, randPos, Quaternion.identity, enemyGroup);
            EnemySubject.Instance.AddEnemy(enemy);

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    private IEnumerator SetSpawnDealy()
    {
        int checkTime = 30;

        while (true)
        {
            curTime += Time.deltaTime;

            if(curTime >= checkTime)
            {
                checkTime += checkTime;
                minDelay *= 0.8f;
                maxDelay *= 0.8f;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
