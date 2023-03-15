using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    [SerializeField] private float missileDmg;
    [SerializeField] private float missileSpeed;
    [HideInInspector] public Transform target;

    private float waitTime = 0f;
    private float distance = 0f;
    private float speed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(transform.position, target.position);
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;

        if(waitTime < 1.5f)
        {
            transform.Translate(transform.forward * missileSpeed * Time.deltaTime);
        }
        else
        {
            speed += Time.deltaTime;
            var t = speed / distance;
            transform.position = Vector3.Lerp(transform.position, target.position, t);
        }

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.GetComponent<Enemy>().GetDamage(missileDmg);
        }
    }
}
