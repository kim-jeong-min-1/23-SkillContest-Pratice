using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType type { get; set; }
    public float speed { get; set; }
    public float damage { get; set; }
    public bool isHit { get; set; }

    public virtual void SetBullet(float speed, float damage, Quaternion rot, EntityType type)
    {
        this.speed = speed;
        this.damage = damage; 
        this.transform.rotation = rot;
        this.isHit = false;
        SetType(type);
    }

    public virtual void SetBullet(float speed, float damage, Quaternion rot)
    {
        this.speed = speed;
        this.damage = damage;
        this.transform.rotation = rot;
        this.isHit = false;
    }


    public void SetType(EntityType type)
    {
        if(EntityType.player == type)
        {
            this.type = BulletType.Player;
        }
        else
        {
            this.type = BulletType.Enemy;
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

public enum BulletType
{
    Player,
    Enemy,
    ChangePlayer
}
