using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : DestroySingleton<BulletPool>
{
    private Queue<Bullet> playerBullets;
    private Queue<Bullet> enemyBullets;

    [SerializeField] private Bullet playerBullet;
    [SerializeField] private Bullet enemyBullet;
    [SerializeField] private Transform bulletGroup;

    private void Awake()
    {
        SetInstance();
        SetVariable();
    }

    private void SetVariable()
    {
        playerBullets = new Queue<Bullet>();
        enemyBullets = new Queue<Bullet>();
        if (!bulletGroup) bulletGroup = new GameObject("BulletGroup").transform;
    }

    public Bullet GetPlayerBullet()
    {
        Bullet n;

        if (playerBullets.Count == 0) n = Instantiate(playerBullet, bulletGroup); 
        else n = playerBullets.Dequeue();
        n.gameObject.SetActive(true);

        return n;
    }

    public Bullet GetEnemyBullet()
    {
        Bullet n;

        if (enemyBullets.Count == 0) n = Instantiate(enemyBullet, bulletGroup);
        else n = enemyBullets.Dequeue();
        n.gameObject.SetActive(true);

        return n;
    }

    public void ReturnToPlayerBullet(Bullet bullet)
    {
        playerBullets.Enqueue(bullet);
    }

    public void ReturnToEnemyBullet(Bullet bullet)
    {
        enemyBullets.Enqueue(bullet);
    }
}
