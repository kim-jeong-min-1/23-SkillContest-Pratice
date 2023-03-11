using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    private Vector3 offset = new Vector3(0, 8f, -12.5f);

    void FixedUpdate()
    {
        Vector3 sumPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, sumPosition, moveSpeed * Time.deltaTime);

        transform.position = smoothPosition;
    }
}