using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : Singleton<PlayerController>
{
    private PlayerInput playerInput;
    [SerializeField] private PlayerStat playerStat;

    [SerializeField] private Image playerHpBar;
    [SerializeField] private Image playerFuelBar;
    [SerializeField] private float shotWaitTime = 0.2f;

    private float playerMaxHp;
    private float playerHp;
    private float playerMaxFuel;
    private float playerFuel;
    private float playerSpeed;
    private float shotCurTime = 0;

    public float HP
    {
        get => playerHp;
        set
        {
            playerHp = value;
        }
    }

    public float Fuel
    {
        get => playerFuel;
        set
        {
            playerFuel = value;
        }
    }

    private void SetPlayer()
    {
        PlayerStat n = new PlayerStat();
        JsonLoader.Save(n, "Player_Stat");
        playerStat = JsonLoader.Load<PlayerStat>("Player_Stat");
        playerInput = GetComponent<PlayerInput>();

        playerHp = playerStat.hp;
        playerFuel = playerStat.fuel;
        playerSpeed = playerStat.speed;

        playerMaxHp = playerHp;
        playerMaxFuel = playerFuel;

        ShooterLevel++;    
    }

    private void Awake()
    {
        SetInstance();
        SetPlayer();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    
    private void Update()
    {
        PlayerShot();
        SetDelta();       
    }

    private void PlayerMovement()
    {
        transform.position += playerInput.moveInput * playerSpeed * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x, Utils.moveLimit.x),
            0f, Mathf.Clamp(transform.position.z, -Utils.moveLimit.y, Utils.moveLimit.y));
    }

    private void PlayerShot()
    {
        if (playerInput.playerShot && shotCurTime >= shotWaitTime)
        {
            shotCurTime = 0;
            Shot();
        }
    }

    private void SetDelta()
    {
        shotCurTime += Time.deltaTime;
    }
}

[System.Serializable]
public class PlayerStat
{
    public float hp;
    public float fuel;
    public float speed;

    public PlayerStat()
    {
        hp = 200;
        fuel = 1000;
        speed = 20;
    }
}
