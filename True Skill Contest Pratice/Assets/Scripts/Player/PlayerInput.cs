using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveInput { get; private set; }

    public bool playerShot
    {
        get => (Input.GetKey(KeyCode.Z));
    }

    void Update()
    {
        moveInput = 
            new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }
}
