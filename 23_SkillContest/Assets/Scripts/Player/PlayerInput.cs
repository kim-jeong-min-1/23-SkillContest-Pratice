using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveInput { get; private set; }
    public bool playerShot { get; private set; }
    public bool playerSkill_1 { get; private set; }
    public bool playerSkill_2 { get; private set; }

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        playerShot = (Input.GetKey(KeyCode.Z));
        playerSkill_1 = (Input.GetKey(KeyCode.X));
        playerSkill_2 = (Input.GetKey(KeyCode.C));
    }
}