using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();
    private void Start()
    {
        rb.velocity = transform.up * bulletSpeed;
    }
}
