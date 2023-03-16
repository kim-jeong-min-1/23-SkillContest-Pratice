using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private PlayerInput playerInput;
    private BulletShooter shooter;

    private float curTime = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shooter = GetComponent<BulletShooter>();    
    }

    private void FixedUpdate()
    {
        PlayerMovement(playerInput.moveInput.normalized);
        
    }

    private void Update()
    {
        PlayerShot();
    }

    private void PlayerMovement(Vector3 moveInput)
    {
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.limit.x, Utils.limit.x), transform.position.y,
            Mathf.Clamp(transform.position.z, -Utils.limit.y, Utils.limit.y));
    }

    private void PlayerShot()
    {
        curTime += Time.deltaTime;

        if (playerInput.playerShot && curTime >= shooter.cool)
        {
            curTime = 0f;
            shooter.fire();
        }
    }
}
