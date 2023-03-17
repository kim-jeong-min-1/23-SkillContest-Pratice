using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image fuelBar;

    private PlayerInput playerInput;
    private BulletShooter shooter;

    private float playerMaxHp;
    [SerializeField] private float playerHp;

    private float playerMaxFuel;
    [SerializeField] private float playerFuel;

    public float Hp
    {
        get => playerHp;
        set
        {
            playerHp = value;

            if (playerHp <= 0) return;
            hpBar.fillAmount = playerHp / playerMaxHp;
        }
    }

    public float Fuel
    {
        get => playerFuel;
        set
        {
            playerFuel = value;
            fuelBar.fillAmount = playerFuel / playerMaxFuel;
        }
    }

    private float curTime = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shooter = GetComponent<BulletShooter>();

        SetInstance();
        playerMaxHp = playerHp;
        playerMaxFuel = playerFuel;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        
    }
    private void Update()
    {
        PlayerShot();
    }

    private void PlayerMovement()
    {
        transform.position += playerInput.moveInput * moveSpeed * Time.deltaTime;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Hp -= other.GetComponent<Bullet>().damage;
        }
    }
}
