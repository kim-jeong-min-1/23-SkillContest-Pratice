using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BulletSubject : DestroySingleton<BulletSubject>
{
    private List<Bullet> bullets;
    private List<Bullet> destroyBullets;

    private void Awake()
    {
        SetInstance();
        SetVariable();
    }

    private void SetVariable()
    {
        bullets = new List<Bullet>();
        destroyBullets = new List<Bullet>();
    }

    public void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }

    private void Update()
    {
        BullettUpdate();
        DestroyBullet();
    }

    private void BullettUpdate()
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
    private void DestroyBullet()
    {
        foreach (var bullet in destroyBullets)
        {
            bullets.Remove(bullet);
            BulletPool.Instance.ReturnBullet(bullet);
        }
        destroyBullets.Clear();
    }

    public void EnemyBulletReflect()
    {
        foreach (var bullet in bullets)
        {
            if(bullet.CompareTag("EnemyBullet"))
            {
                bullet.Reflection();
                bullet.ChangeType(EntityType.Player);
                bullet.ChangeColor(Color.cyan);
                bullet.damage = bullet.damage / 5;
            }
        }
    }
}
