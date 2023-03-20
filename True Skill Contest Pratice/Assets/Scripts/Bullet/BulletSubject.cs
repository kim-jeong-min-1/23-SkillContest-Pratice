using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSubject : Singleton<BulletSubject>
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
        DestroyBullet();
    }

    public void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }

    private void BulletUpdate()
    {
        foreach (var bullet in bullets)
        {
            bullet.BulletMovement();
            if (bullet.isHit || Utils.ObjectOutCheck(bullet.transform.position))
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
            bullet.gameObject.SetActive(false);

            if (bullet.type == BulletType.Player)
            {
                
                BulletPool.Instance.ReturnToPlayerBullet(bullet);
            }
            else
            {
                BulletPool.Instance.ReturnToEnemyBullet(bullet);
            }
        }
        destroyBullets.Clear();
    }

    public void ReflectToEnemyBullet()
    {
        foreach(var bullet in bullets)
        {
            if (bullet.type == BulletType.Enemy)
            {
                bullet.Reflection();
                bullet.SetSpriteColor(Color.cyan);
                bullet.speed = 50;
                bullet.damage = bullet.damage / 5;
                bullet.type = BulletType.ChangePlayer;
            }
        }
    }
}
