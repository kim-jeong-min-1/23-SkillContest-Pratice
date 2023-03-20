using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private EntityType type;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] public float cool;
    public Bullet curFireBullet { get; set; }

    public BulletShooter(float damage, float speed, float cool, EntityType type)
    {
        this.damage = damage;
        this.speed = speed;
        this.cool = cool;
        this.type = type;   
    }

    public void fire()
    {
        Bullet bullet;
        if (type == EntityType.player) bullet = BulletPool.Instance.GetPlayerBullet();
        else bullet = BulletPool.Instance.GetEnemyBullet();

        bullet.transform.position = transform.position;
        bullet.SetBullet(speed, damage, transform.rotation, type);
    
        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
    }

    public void fire(Quaternion rot)
    {
        Bullet bullet;
        if (type == EntityType.player) bullet = BulletPool.Instance.GetPlayerBullet();
        else bullet = BulletPool.Instance.GetEnemyBullet();

        bullet.transform.position = transform.position;
        bullet.SetBullet(speed, damage, rot, type);

        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
    }              
}

public enum EntityType
{
    player,
    enemy,
    boss
}
