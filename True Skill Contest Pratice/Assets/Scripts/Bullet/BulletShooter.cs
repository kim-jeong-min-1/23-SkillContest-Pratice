using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private EntityType type;
    [SerializeField] private Bullet bulletObj;

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] public float cool;

    public BulletShooter(float damage, float speed, float cool, EntityType type)
    {
        this.damage = damage;
        this.speed = speed;
        this.cool = cool;
        this.type = type;   
    }

    public void fire()
    {
        Bullet bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.SetBullet(speed, damage, transform.rotation, type);
    }
    public void fire(Quaternion rot)
    {
        Bullet bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.SetBullet(speed, damage, rot, type);
    }
    public void fire(Vector3 pos, Quaternion rot)
    {
        Bullet bullet = Instantiate(bulletObj, pos, Quaternion.identity);
        bullet.SetBullet(speed, damage, rot, type);
    }               
}

public enum EntityType
{
    player,
    enemy,
    boss
}
