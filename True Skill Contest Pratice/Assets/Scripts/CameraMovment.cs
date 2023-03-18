using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject target;

    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
        if (!target) target = FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z) + offset;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraSpeed * Time.deltaTime);

        mainCamera.transform.position = 
            new Vector3(Mathf.Clamp(mainCamera.transform.position.x, -Utils.moveLimit.x + 25f, Utils.moveLimit.x - 25f),
            mainCamera.transform.position.y, Mathf.Clamp(mainCamera.transform.position.z, -Utils.moveLimit.y + 2f, Utils.moveLimit.y - 40f));
    }
}
