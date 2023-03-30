using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed { get; set; }
    public float damage { get; set; }
    public bool isHit { get; set; }
    public bool isStop { get; set; }
    public BulletType type { get; set; }

    public void SetBullet(float speed, float damage, Quaternion rot, EntityType type)
    {
        this.speed = speed;
        this.damage = damage;
        this.transform.rotation = rot;
        isHit = false;
        isStop = false;
        ChangeType(type);
    }

    public void ChangeType(EntityType type)
    {
        if(type == EntityType.Player)
        {
            this.type = BulletType.Player;
        }
        else
        {
            this.type = BulletType.Enemy;
        }
    }

    public void ChangeColor(Color color)
    {
        var sprite = transform.Find("sprite").GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void Reflection()
    {
        var reflect = transform.eulerAngles + new Vector3(0, 180f, 0);
        transform.rotation = Quaternion.Euler(reflect);
    }

    public void BulletMovement()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

}

public enum BulletType
{
    Player,
    Enemy
}