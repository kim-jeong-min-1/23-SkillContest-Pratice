using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject model;

    private Vector3 moveInput;
    private Vector3 defaultRotation = new Vector3(0, 90, 0);

    private float rotateX = 0;
    private float rotateZ = 0;

    void FixedUpdate()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        PlayerRotation(moveInput);
        
        var moveDir = transform.forward + moveInput;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void PlayerRotation(Vector3 moveInput)
    {
        if (moveInput != Vector3.zero)
        {
            rotateX = Mathf.Clamp(rotateX + moveInput.x * rotateSpeed * Time.deltaTime, -30, 30);
            rotateZ += moveInput.y * rotateSpeed * Time.deltaTime;
            model.transform.eulerAngles = new Vector3(rotateX, model.transform.eulerAngles.y, -rotateZ);
        }
        else
        {
            model.transform.eulerAngles = 
                Vector3.Lerp(model.transform.eulerAngles, defaultRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
