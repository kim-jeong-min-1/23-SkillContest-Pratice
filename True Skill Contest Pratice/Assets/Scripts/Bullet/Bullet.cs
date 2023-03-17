using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed { get; set; }
    public float damage { get; set; }

    public virtual void SetBullet(float speed, float damage, Quaternion rotation, EntityType type)
    {
        this.speed = speed;
        this.damage = damage; 
        this.transform.rotation = rotation;
        SetTag(type);
    }

    public virtual void SetBullet(float speed, float damage, Quaternion rotation)
    {
        this.speed = speed;
        this.damage = damage;
        this.transform.rotation = rotation;
    }


    private void SetTag(EntityType type)
    {
        if(EntityType.player == type)
        {
            this.tag = "PlayerBullet";
        }
        else
        {
            this.tag = "EnemyBullet";
        }
    }

    public virtual void BulletMovement()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    private void Update()
    {
        BulletMovement();
    }


    public void GetDamage()
    {

    }
}
