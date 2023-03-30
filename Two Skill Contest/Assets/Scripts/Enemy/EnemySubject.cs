using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubject : DestroySingleton<EnemySubject>
{
    private List<Enemy> enemies;
    private List<Enemy> destroyEnemies;
    [SerializeField] private ParticleSystem explosion;
    public int enemyCount { get; set; }


    private void Awake()
    {
        SetInstance();
        SetVariable();
    }

    private void SetVariable()
    {
        enemies = new List<Enemy>();
        destroyEnemies = new List<Enemy>();
    }

    private void Update()
    {
        EnemyUpdate();
        DestroyEnemy();
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemyCount++;
    }

    private void EnemyUpdate()
    {
        foreach (var enemy in enemies)
        {
            if(enemy.isDie || Utils.OutCheck(enemy.transform.position))
            {
                destroyEnemies.Add(enemy);
            }
        }
    }

    private void DestroyEnemy()
    {
        foreach (var enemy in destroyEnemies)
        {
            enemies.Remove(enemy);

            Instantiate(explosion, enemy.transform.position, Quaternion.identity);
            GameManager.Instance.score += enemy.enemyScore;
            Destroy(enemy.gameObject);
        }
        destroyEnemies.Clear();
    }

    public Transform NearToTargetEnemy(Vector3 target)
    {
        float dis = 999;
        Transform near = null;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            var n = Vector3.Distance(target, enemy.transform.position);
            if(dis > n)
            {
                dis = n;
                near = enemy.transform;
            }
        }
        return near;
    }
}
