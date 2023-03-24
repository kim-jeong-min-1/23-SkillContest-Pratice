using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubject : DestroySingleton<EnemySubject>
{
    [SerializeField] private ParticleSystem enemyDieEffect;
    private List<Enemy> curEnemies;
    private List<Enemy> dieEnemies;

    public int enemyCount { get; set; }

    private void Awake()
    {
        SetInstance();
        SetVariable();
    }

    private void SetVariable()
    {
        curEnemies = new List<Enemy>();
        dieEnemies = new List<Enemy>();
    }

    private void Update()
    {
        EnemyUpdate();
        EnemyDie();
    }

    private void EnemyUpdate()
    {
        foreach (var enemy in curEnemies)
        {
            if (enemy == null) continue;
            if (enemy.isDie || Utils.ObjectOutCheck(enemy.transform.position))
            {
                dieEnemies.Add(enemy);
            }
        }
    }

    private void EnemyDie()
    {
        foreach (var enemy in dieEnemies)
        {
            if (enemy.isDie) EnemyDie(enemy.score, enemy.transform.position);

            Destroy(enemy.gameObject);
            curEnemies.Remove(enemy);
        }
        dieEnemies.Clear();
    }

    public void AddEnemy(Enemy enemy)
    {
        curEnemies.Add(enemy);
        enemyCount++;
    }

    private void EnemyDie(int score ,Vector3 pos)
    {
        Instantiate(enemyDieEffect, pos, Quaternion.identity);
        GameManager.Instance.AddScore(score);

        var randProb = Random.Range(1, 21);
        if (randProb <= 2) PlayerSkillSystem.Instance.SelectSkill();
    }

    public Transform NearToTargetEnemy(Vector3 target)
    {
        Transform nearEnemy = null;
        float nearDis = 1000;

        foreach (var enemy in curEnemies)
        {
            if (enemy == null) continue;

            var dis = Vector3.Distance(target, enemy.transform.position);
            if (dis < nearDis)
            {
                nearDis = dis;
                nearEnemy = enemy.transform;
            }
        }
        return nearEnemy;
    }
}
