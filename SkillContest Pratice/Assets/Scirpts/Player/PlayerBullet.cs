using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDmage;
    [SerializeField] private float bulletTime;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
        StartCoroutine(bulletDuration());
    }

    private IEnumerator bulletDuration()
    {
        yield return new WaitForSeconds(bulletTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GetDamage(bulletDmage);
            Destroy(gameObject);
        }
    }
}
