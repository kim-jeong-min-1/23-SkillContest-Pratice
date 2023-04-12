using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : DestorySingleton<BulletPool>
{
    private Queue<Bullet> enemyBullets;
    private Queue<Bullet> playerBullets;

    [SerializeField] private Bullet enemyBulletObj;
    [SerializeField] private Bullet playerBulletObj;

    [SerializeField] private Transform bulletGroup;

    private void Awake()
    {
        SetInstance();
        enemyBullets = new Queue<Bullet>();
        playerBullets = new Queue<Bullet>();
    }

    public Bullet GetEnemyBullet()
    {
        Bullet n;

        if (enemyBullets.Count == 0) n = Instantiate(enemyBulletObj, bulletGroup);
        else n = enemyBullets.Dequeue();

        n.gameObject.SetActive(true);
        return n;
    }
    public void ReturnEnemyBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        enemyBullets.Enqueue(bullet);
    }

    public Bullet GetPlayerBullet()
    {
        Bullet n;

        if (playerBullets.Count == 0) n = Instantiate(playerBulletObj, bulletGroup);
        else n = playerBullets.Dequeue();

        n.gameObject.SetActive(true);
        return n;
    }
    public void ReturnPlayerBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        playerBullets.Enqueue(bullet);
    }

}
