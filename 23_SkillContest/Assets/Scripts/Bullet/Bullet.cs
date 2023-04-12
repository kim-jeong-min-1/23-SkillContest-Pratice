using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed { get; set; }
    public float damage { get; set; }
    public bool isHit { get; set; } = false;
    public bool isStop { get; set; } = false;
    public bool isReflect { get; set; } = false;

    public void SetBullet(float speed, float damage, Quaternion rot, EntityType type)
    {
        this.speed = speed;
        this.damage = damage;
        isHit = false;
        isStop = false;
        isReflect = false;
        transform.rotation = rot;
        ChangeType(type);
    }

    public void SetBullet(float speed, float damage, EntityType type)
    {
        this.speed = speed;
        this.damage = damage;
        isHit = false;
        isStop = false;
        isReflect = false;
        ChangeType(type);
    }

    public void SetColor(Color color)
    {
        var sprite = transform.Find("sprite").GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void ChangeType(EntityType type)
    {
        if(type == EntityType.Player)
        {
            this.tag = "PlayerBullet";
        }
        else
        {
            this.tag = "EnemyBullet";
        }

    }

    public void Reflection()
    {
        var rot = transform.eulerAngles + new Vector3(0, 180f, 0);
        transform.rotation = Quaternion.Euler(rot);
    }

    public void BulletMovement()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
