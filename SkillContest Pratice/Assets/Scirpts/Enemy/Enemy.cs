using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float viewRadius;

    [SerializeField] protected Slider enemyHpBar;
    [SerializeField] protected Transform player;

    public float Hp
    {
        get => enemyHp;
        set
        {
            enemyHp = value;
            enemyHpBar.value = value;

            if (enemyHp <= 0) Destroy(gameObject);
        }
    }

    private void Awake()
    {
        enemyHpBar.maxValue = enemyHp;
        enemyHpBar.value = enemyHpBar.maxValue;

        StartCoroutine(EnemyAI_Update());
    }


    protected abstract IEnumerator EnemyAI_Update();
    public void GetDamage(float damage)
    {
        Hp -= damage;
    }
}
