using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHp;
    [SerializeField] private float enemySpeed;

    private BulletShooter shooter;

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
}
