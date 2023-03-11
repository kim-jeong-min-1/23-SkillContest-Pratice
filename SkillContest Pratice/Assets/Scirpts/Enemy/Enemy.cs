using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float enemyHp;
    public float Hp
    {
        get => enemyHp;
        set
        {
            if (enemyHp <= 0) Destroy(gameObject);
            else enemyHp = value;
        }
    }

    public void GetDamage(float damage)
    {
        Hp -= damage;
    }
    protected abstract IEnumerator EnemyAI_Update();
}
