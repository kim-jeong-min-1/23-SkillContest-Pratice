using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private EntityType type;
    public Bullet curFireBullet { get; private set; }


    public void Fire()
    {
        Bullet bullet;
        if (type == EntityType.Player) bullet = BulletPool.Instance.GetPlayerBullet();
        else bullet = BulletPool.Instance.GetEnemyBullet();

        bullet.SetBullet(speed, damage, type);
        bullet.transform.position = transform.position;

        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
    }

    public void Fire(Quaternion rot)
    {
        Bullet bullet;
        if (type == EntityType.Player) bullet = BulletPool.Instance.GetPlayerBullet();
        else bullet = BulletPool.Instance.GetEnemyBullet();

        bullet.SetBullet(speed, damage, rot, type);
        bullet.transform.position = transform.position;

        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
    }
}

public enum EntityType
{
    Player,
    Enemy,
    Boss
}