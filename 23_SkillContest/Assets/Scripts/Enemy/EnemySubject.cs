using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubject : DestorySingleton<EnemySubject>
{
    public int enemyCount = 0;
    private List<Enemy> enemies;
    private List<Enemy> deadEnemies;

    [SerializeField] private ParticleSystem dieEffect;

    private void Awake()
    {
        SetInstance();
        enemies = new List<Enemy>();
        deadEnemies = new List<Enemy>();
    }

    private void Update()
    {
        EnemyUpdate();
        DestroyEnemy();
    }

    private void EnemyUpdate()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.isDie || Utils.OutCheck(enemy.transform.position))
            {
                deadEnemies.Add(enemy);
            }
        }
    }

    private void DestroyEnemy()
    {
        foreach (var enemy in deadEnemies)
        {
            if (enemy.isDie)
            {
                GameManager.Instance.Score += enemy.score;
                SoundManager.Instance.PlaySfx(SoundEffect.Boom, 0.7f);

                var rand = Random.Range(1, 11);
                if (rand == 1) PlayerController.Instance.playerSkill.SelectSkill();
            }

            enemies.Remove(enemy);
            Instantiate(dieEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
        }
        deadEnemies.Clear();
    }


    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }


    public Transform NearToTargetEnemy(Vector3 target)
    {
        Transform near = null;
        float dis = 999;

        foreach (var enemy in enemies)
        {
            var tempDis = Vector3.Distance(enemy.transform.position, target);
            if (tempDis < dis)
            {
                dis = tempDis;
                near = enemy.transform;
            }
        }

        return near;
    }

    public void DestroyAllEnemy()
    {
        foreach (var enemy in enemies)
        {
            enemy.isDie = true;
        }
    }
}
