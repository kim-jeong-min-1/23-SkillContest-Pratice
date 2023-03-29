using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private EntityType type;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    public Bullet curFireBullet { get; set; }

    public Bullet Fire()
    {
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = transform.position;

        bullet.SetBullet(speed, damage, Quaternion.identity, type);

        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
        return bullet;
    }

    public Bullet Fire(Quaternion rot)
    {
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = transform.position;

        bullet.SetBullet(speed, damage, rot, type);

        curFireBullet = bullet;
        BulletSubject.Instance.AddBullet(bullet);
        return bullet;
    }
}

public enum EntityType
{
    Player,
    Enemy,
    Boss
}
