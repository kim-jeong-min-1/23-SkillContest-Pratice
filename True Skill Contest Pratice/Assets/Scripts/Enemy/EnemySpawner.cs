using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private Transform enemyGroup;
    [SerializeField] private float maxDelay;
    [SerializeField] private float minDelay;

    void Start()
    {
        StartCoroutine(Enemy_Spawner());
    }  

    private IEnumerator Enemy_Spawner()
    {        
        while (!GameManager.Instance.qusetComplete)
        {
            int randEnemy = Random.Range(0, enemies.Count);         

            Vector3 spawnPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);
            Enemy enemy = Instantiate(enemies[randEnemy], spawnPos, Quaternion.identity, enemyGroup);
            EnemySubject.Instance.AddEnemy(enemy);

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
