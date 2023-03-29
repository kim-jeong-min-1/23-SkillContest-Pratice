using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPool : DestroySingleton<BulletPool>
{
    private Queue<Bullet> bullets;

    [SerializeField] private Bullet bulletObj;
    [SerializeField] private Transform bulletGroup;

    private void Awake()
    {
        SetInstance();
        bullets = new Queue<Bullet>();
    }

    public Bullet GetBullet()
    {
        Bullet n;

        if (bullets.Count != 0) n = bullets.Dequeue();
        else n = Instantiate(bulletObj, bulletGroup);

        n.gameObject.SetActive(true);
        return n;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullets.Enqueue(bullet);
    }

}
