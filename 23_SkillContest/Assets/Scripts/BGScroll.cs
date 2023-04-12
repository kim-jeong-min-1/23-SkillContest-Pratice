using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 returnPos;
    [SerializeField] private float returnZ;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        if(transform.position.z <= returnZ)
        {
            transform.position = returnPos;
        }
    }
}
