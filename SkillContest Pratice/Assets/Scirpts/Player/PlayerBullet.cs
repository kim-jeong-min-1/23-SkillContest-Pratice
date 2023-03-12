using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDmage;
    [SerializeField] private float bulletTime;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        StartCoroutine(bulletDuration());       
        StartCoroutine(BulletUpdate());
    }

    private IEnumerator BulletUpdate()
    {
        RaycastHit hit;
        while (this.gameObject != null)
        {
            //var nextPos = rb.position + rb.velocity * Time.deltaTime;
            Physics.Raycast(rb.position, transform.forward, out hit, bulletSpeed * Time.deltaTime);

            if(hit.collider != null)
            {
                if(hit.collider.TryGetComponent(out Enemy enemy))
                {
                    enemy.GetDamage(bulletDmage);
                    Destroy(gameObject);
                    yield break; 
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator bulletDuration()
    {
        yield return new WaitForSeconds(bulletTime);
        Destroy(gameObject);
        yield break;
    }
}
