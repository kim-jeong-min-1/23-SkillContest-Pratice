using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region È¸Àü °ª
    readonly private float MAX_ROTATEX = 30f;
    readonly private float MIN_ROTATEX = -30f;
    readonly private float MAX_ROTATEZ = 10f;
    readonly private float MIN_ROTATEZ = -30f;
    #endregion

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float returnRotateSpeed;
    [SerializeField] private GameObject model;

    private Vector3 moveInput;
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
            rotateX = Mathf.Clamp(rotateX + moveInput.x * rotateSpeed * Time.deltaTime, MIN_ROTATEX, MAX_ROTATEX);
            rotateZ = Mathf.Clamp(rotateZ + moveInput.y * rotateSpeed * Time.deltaTime, MIN_ROTATEZ, MAX_ROTATEZ);           
        }
        else
        {
            rotateX = Mathf.Lerp(rotateX, 0, returnRotateSpeed * Time.deltaTime);
            rotateZ = Mathf.Lerp(rotateZ, 0, returnRotateSpeed * Time.deltaTime);
        }

        model.transform.eulerAngles = new Vector3(rotateX, model.transform.eulerAngles.y, -rotateZ);
    }
}
