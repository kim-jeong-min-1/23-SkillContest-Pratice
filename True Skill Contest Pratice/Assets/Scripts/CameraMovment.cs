using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset;

    private Transform target;

    void Awake()
    {
        if(!target) target = FindObjectOfType<PlayerController>().transform;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);

        transform.position = 
            new Vector3(Mathf.Clamp(transform.position.x, -Utils.limit.x + 25f, Utils.limit.x - 25f),
            transform.position.y, Mathf.Clamp(transform.position.z, -Utils.limit.y -2f, Utils.limit.y - 40f));
    }
}
