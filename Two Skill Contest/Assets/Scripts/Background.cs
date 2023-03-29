using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float returnPosZ;
    [SerializeField] private Vector3 returnPos;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if(transform.position.z < returnPosZ)
        {
            transform.position = returnPos;
        }
    }
}
