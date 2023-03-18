using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image fuelBar;

    private PlayerInput playerInput;
    private BulletShooter shooter;
    private Transform target;
    private Transform model;

    private float playerHp;
    private float playerMaxHp;
    private float playerFuel;
    private float playerMaxFuel;
    private float playerSpeed;

    private float shotCurTime = 0f;
    private float hitCurTime = 0f;
    private float playerHitDelayTime = 1f;
    private float rotaionSpeed = 0.25f;
    private Quaternion targetDir;

    public float Hp
    {
        get => playerHp;
        set
        {
            playerHp = value;
            if (playerHp > playerMaxHp) playerHp = playerMaxHp;
            if (playerHp < 0) playerHp = 0;

            hpBar.fillAmount = playerHp / playerMaxHp;
        }
    }

    public float Fuel
    {
        get => playerFuel;
        set
        {
            playerFuel = value;
            if (playerFuel > playerMaxFuel) playerFuel = playerMaxFuel;
            if (playerFuel < 0) playerFuel = 0;

            fuelBar.fillAmount = playerFuel / playerMaxFuel;
        }
    }

    private void SetPlayer()
    {
        playerStat = JsonLoader.Load<PlayerStat>("Player_Stat");
        playerInput = GetComponent<PlayerInput>();
        shooter = GetComponent<BulletShooter>();
        model = gameObject.transform.Find("model").transform;

        playerHp = playerStat.hp;
        playerFuel = playerStat.fuel;
        playerSpeed = playerStat.speed;

        playerMaxHp = playerHp;
        playerMaxFuel = playerFuel;
    }

    public void Awake()
    {        
        SetInstance();
        SetPlayer();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotate();
    }
    private void Update()
    {
        FindToTarget();
        PlayerShot();
        SetDeltaTime();
    }

    private void PlayerMovement()
    {
        transform.position += playerInput.moveInput * playerSpeed * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x, Utils.moveLimit.x), transform.position.y,
            Mathf.Clamp(transform.position.z, -Utils.moveLimit.y, Utils.moveLimit.y));
    }
    private void PlayerRotate()
    {
        Quaternion targetRot = Quaternion.Slerp(model.rotation, targetDir, rotaionSpeed);
        model.rotation = targetRot;
    }
    private void FindToTarget()
    {
        target = EnemySubject.Instance.NearToTargetEnemy(transform.position);

        if(target != null)
        {
            var dis = target.position - transform.position;
            dis.y = 0;

            targetDir = Quaternion.LookRotation(dis);
        }
    }
    private void PlayerShot()
    {
        if (!playerInput.playerShot || shotCurTime < shooter.cool) return;
        shotCurTime = 0f;

        if (target != null)
        {
            shooter.fire(targetDir);
        }
        else
        {
            shooter.fire();
        }       
    }

    private void SetDeltaTime()
    {
        shotCurTime += Time.deltaTime;
        hitCurTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if (hitCurTime >= playerHitDelayTime)
            {
                hitCurTime = 0f;
                Hp -= other.GetComponent<Bullet>().damage;
                UIManager.Instance.PlayerHitUIEffect(playerHitDelayTime);
            }
        }
    }
}

[System.Serializable]
public class PlayerStat
{
    public float hp;
    public float speed;
    public float fuel;

    public PlayerStat()
    {
        hp = 200;
        fuel = 1000;
        speed = 45;
    }
}
