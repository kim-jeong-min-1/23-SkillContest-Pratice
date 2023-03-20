using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed { get; set; }
    public float damage { get; set; }
    public bool isHit { get; set; }

    public virtual void SetBullet(float speed, float damage, Quaternion rot, EntityType type)
    {
        this.speed = speed;
        this.damage = damage; 
        this.transform.rotation = rot;
        this.isHit = false;
        SetTag(type);
    }

    public virtual void SetBullet(float speed, float damage, Quaternion rot)
    {
        this.speed = speed;
        this.damage = damage;
        this.transform.rotation = rot;
        this.isHit = false;
    }


    public void SetTag(EntityType type)
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

    public void SetSpriteColor(Color color)
    {
        var sprite = gameObject.transform.Find("sprite").GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void Reflection()
    {
        var refelct = transform.eulerAngles + new Vector3(0, -180f, 0);
        transform.rotation = Quaternion.Euler(refelct);
    }

    public virtual void BulletMovement()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
