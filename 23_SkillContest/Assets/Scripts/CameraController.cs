using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    void Start()
    {
        if (!target) target = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z) + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x + 10f, Utils.moveLimit.x - 10f), offset.y,
            Mathf.Clamp(transform.position.z, -Utils.moveLimit.y, Utils.moveLimit.y - 15f));
    }
}
