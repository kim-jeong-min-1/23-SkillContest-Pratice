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

    public void fire(Vector3 Pos = default, Quaternion Rot = default)
    {
        Pos = (Pos == default(Vector3)) ? transform.position : Pos;
        Rot = (Rot == default(Quaternion)) ? transform.rotation : Rot;

        Bullet bullet = Instantiate(bulletObj, Pos, Quaternion.identity);
        bullet.SetBullet(speed, damage, Rot, type);
    }
}

public enum EntityType
{
    player,
    enemy,
    boss
}
