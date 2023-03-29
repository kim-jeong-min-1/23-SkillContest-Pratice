using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 cameraOffset;

    private Transform playerObj;

    private void Start()
    {
        playerObj = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(playerObj.position.x, playerObj.position.y, playerObj.position.z) + cameraOffset;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x + 10, Utils.moveLimit.x - 10), transform.position.y,
            Mathf.Clamp(transform.position.z, -Utils.moveLimit.y + 2, Utils.moveLimit.y - 15));

    }
}
