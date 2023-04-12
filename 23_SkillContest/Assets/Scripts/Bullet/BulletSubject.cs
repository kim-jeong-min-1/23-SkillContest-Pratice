using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSubject : DestorySingleton<BulletSubject>
{
    private List<Bullet> bullets;
    private List<Bullet> destroyBullets;

    private void Awake()
    {
        SetInstance();
        bullets = new List<Bullet>();
        destroyBullets = new List<Bullet>();
    }

    private void Update()
    {
        BulletUpdate();
        DestoryBullet();
    }


    private void BulletUpdate()
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.isStop) bullet.BulletMovement();
            if (bullet.isHit || Utils.OutCheck(bullet.transform.position))
            {
                destroyBullets.Add(bullet);
            }
        }
    }

    private void DestoryBullet()
    {
        foreach (var bullet in destroyBullets)
        {
            bullets.Remove(bullet);
   
            if (bullet.CompareTag("EnemyBullet") || bullet.isReflect)
            {
                BulletPool.Instance.ReturnEnemyBullet(bullet);
            }
            else 
            {
                BulletPool.Instance.ReturnPlayerBullet(bullet);
            }
        }
        destroyBullets.Clear();
    }

    public void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }

    public void ChangeToEnemyBullet()
    {
        foreach (var bullet in bullets)
        {
            if (bullet.CompareTag("EnemyBullet"))
            {
                bullet.ChangeType(EntityType.Player);
                bullet.Reflection();
                bullet.SetColor(Color.cyan);
                bullet.damage = bullet.damage / 5f;
                bullet.isReflect = true;
            }
        }
    }
}
