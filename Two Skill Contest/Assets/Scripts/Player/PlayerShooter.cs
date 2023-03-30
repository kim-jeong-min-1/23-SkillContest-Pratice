using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public partial class PlayerController
{
    [SerializeField] private BulletShooter playerShooterObj;

    private readonly int MaxShooterLevel = 4;
    private List<BulletShooter> shooters = new List<BulletShooter>();

    private int shooterLevel = 0;
    public int ShooterLevel
    {
        get => shooterLevel;
        set
        {
            if (value < MaxShooterLevel)
            {
                shooterLevel = value;
                SetPlayerShooter();
            }
            else return; // 점수추가 
        }
    }

    private void SetPlayerShooter()
    {
        DestroyShooter();

        if (ShooterLevel == 1)
        {
            AddShooter(Vector3.zero);
        }
        else if (shooterLevel == 2)
        {
            AddShooter(Vector3.right * 1.5f);
            AddShooter(-Vector3.right * 1.5f);
        }
        else if (shooterLevel == 3)
        {
            AddShooter(Vector3.forward * 2.5f);
            AddShooter(Vector3.right * 3f);
            AddShooter(-Vector3.right * 3f);
        }
        else if (shooterLevel == 4)
        {
            AddShooter(Vector3.forward * 2.5f + -Vector3.right * 1.5f);
            AddShooter(Vector3.forward * 2.5f + Vector3.right * 1.5f);
            AddShooter(Vector3.right * 4.5f);
            AddShooter(-Vector3.right * 4.5f);
        }
    }

    private void AddShooter(Vector3 pos)
    {
        var shooter = Instantiate(playerShooterObj, transform);
        shooter.transform.localPosition = pos;

        shooters.Add(shooter);
    }

    private void DestroyShooter()
    {
        shooters.Clear();
    }

    public void Shot()
    {
        for (int i = 0; i < shooters.Count; i++)
        {
            shooters[i].Fire();
            shooters[i].curFireBullet.ChangeColor(Color.cyan);
        }
    }
    public void Shot(Quaternion rot)
    {
        for (int i = 0; i < shooters.Count; i++)
        {
            shooters[i].Fire(rot);
            shooters[i].curFireBullet.ChangeColor(Color.cyan);
        }
    }
}
