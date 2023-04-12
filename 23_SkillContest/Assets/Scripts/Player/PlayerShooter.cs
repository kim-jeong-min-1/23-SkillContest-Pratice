using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private readonly int MaxShooterLevel = 4;
    private int level;

    private List<BulletShooter> shooters = new List<BulletShooter>();
    [SerializeField] private BulletShooter playerShooter;

    public int ShooterLevel
    {
        get => level;
        set
        {
            if (value <= MaxShooterLevel)
            {
                level = value;
                SetShooter();
            }
            else GameManager.Instance.Score += 200;
        }
    }

    private void SetShooter()
    {
        DestroyAllShooter();
        if (ShooterLevel == 1)
        {
            AddPlayerShooter(Vector3.forward);
        }
        else if (ShooterLevel == 2)
        {
            AddPlayerShooter(Vector3.right * 0.5f);
            AddPlayerShooter(Vector3.right * -0.5f);
        }
        else if (ShooterLevel == 3)
        {
            AddPlayerShooter(Vector3.right * 1f);
            AddPlayerShooter(Vector3.right * -1f);
            AddPlayerShooter(Vector3.forward * 1f);
        }
        else if (ShooterLevel == 4)
        {
            AddPlayerShooter(Vector3.right * 1.5f);
            AddPlayerShooter(Vector3.right * -1.5f);
            AddPlayerShooter(Vector3.forward * 1f + Vector3.right * 0.5f);
            AddPlayerShooter(Vector3.forward * 1f + Vector3.right * -0.5f);
        }

    }
    private void AddPlayerShooter(Vector3 Pos)
    {
        BulletShooter shooter = Instantiate(playerShooter, transform);
        shooter.transform.localPosition = Pos;

        shooters.Add(shooter);
    }

    private void DestroyAllShooter()
    {
        shooters.Clear();
    }

    public void ShotBullet()
    {
        for (int i = 0; i < shooters.Count; i++) shooters[i].Fire();
    }

    public void ShotBullet(Quaternion rot)
    {
        for (int i = 0; i < shooters.Count; i++) shooters[i].Fire(rot);
    }
}
