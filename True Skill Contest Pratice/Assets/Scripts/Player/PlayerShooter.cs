using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public partial class PlayerController
{
    [SerializeField] private BulletShooter shooterObj;
    public List<BulletShooter> shooters { get; set; }

    private readonly float shotCool = 0.15f;
    private readonly int maxShooterLevel = 4;
    private float curShooterLevel;
    public float ShooterLevel
    {
        get => curShooterLevel;
        set
        {
            if (value <= maxShooterLevel && value != curShooterLevel)
            {
                curShooterLevel = value;
                SetShooter();
            }
        }
    }

    private void SetShooter()
    {
        DestroyShooter();
        if (ShooterLevel == 1)
        {           
            AddPlayerShooter(Vector3.zero);
        }
        if (ShooterLevel == 2)
        {
            AddPlayerShooter(Vector3.right * 1.5f);
            AddPlayerShooter(Vector3.right * -1.5f);
        }
        if (ShooterLevel == 3)
        {
            AddPlayerShooter(Vector3.zero + Vector3.forward * 2.5f);
            AddPlayerShooter(Vector3.right * 3);
            AddPlayerShooter(Vector3.right * -3);
        }
        if (ShooterLevel == 4)
        {
            AddPlayerShooter(Vector3.right * 1.5f + Vector3.forward * 2.5f);
            AddPlayerShooter(Vector3.right * -1.5f + Vector3.forward * 2.5f);
            AddPlayerShooter(Vector3.right * 4.5f);
            AddPlayerShooter(Vector3.right * -4.5f);
        }
    }

    private void AddPlayerShooter(Vector3 pos)
    {
        BulletShooter n = Instantiate(shooterObj, transform);
        n.cool = shotCool;
        n.transform.localPosition = pos;

        shooters.Add(n);
    }

    private void DestroyShooter()
    {
        shooters.Clear();
    }

    public void ShotBullet()
    {
        for (int i = 0; i < shooters.Count; i++) shooters[i].fire();
    }

    public void ShotBullet(Quaternion rot)
    {
        for (int i = 0; i < shooters.Count; i++) shooters[i].fire(rot);
    }
}
