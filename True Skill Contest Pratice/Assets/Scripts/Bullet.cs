using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletType type;
    private float speed;
    private float damage;

    public float Speed { get { return speed; }  set { speed = value; } }
    public float Damage { get { return damage; } set { damage = value; } }

    public virtual void SetBullet(float speed, float damage, Quaternion rotation, BulletType type)
    {
        this.speed = speed;
        this.damage = damage; 
        this.type = type;
        this.transform.rotation = rotation;
        SetTag(type);
    }

    private void SetTag(BulletType type)
    {
        if(BulletType.playerBullet == type)
        {
            this.tag = "PlayerBullet";
        }
        else
        {
            this.tag = "EnemyBullet";
        }
    }

    public virtual void BulletUpdate()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    private void Update()
    {
        BulletUpdate();
    }


    public void GetDamage()
    {

    }
}

public enum BulletType
{
    playerBullet,
    enemyBullet,
    bossBullet
}
