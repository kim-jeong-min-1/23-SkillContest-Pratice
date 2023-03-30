using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private PlayerStat playerStat;
    private PlayerInput playerInput;
    private PlayerSkillSystem playerSkill;

    [Space (20f)]
    [SerializeField] private Image playerHpBar;
    [SerializeField] private Image playerFuelBar;

    [Space(20f)]
    [SerializeField] private Transform playerModel;
    public GameObject playerRayzer;
    public GameObject playerShield;
    private Transform target;

    private float playerMaxHp;
    private float playerHp;
    private float playerMaxFuel;
    private float playerFuel;
    private float playerSpeed;

    private float shotCurTime = 0;
    private float hitCurTime = 0;
    private float skill_1CurTime = 0;
    private float skill_2CurTime = 0;

    private readonly float shotWaitTime = 0.2f;
    private readonly float hitWaitTime = 1f;
    private readonly float playerSkill_1Cool = 8f;
    private readonly float playerSkill_2Cool = 15f;

    private Quaternion targetDir;
    public bool isInvis { get; set; } = false;
    public bool isDie { get; set; } = false;

    public float HP
    {
        get => playerHp;
        set
        {
            playerHp = value;
            if (playerHp < 0) playerHp = 0;
            else if (playerHp > playerMaxHp) playerHp = playerMaxHp;

            playerHpBar.fillAmount = playerHp / playerMaxHp;
        }
    }
    public float Fuel
    {
        get => playerFuel;
        set
        {
            playerFuel = value;
            if (playerFuel < 0) playerFuel = 0;
            else if (playerFuel > playerMaxFuel) playerFuel = playerMaxFuel;

            playerFuelBar.fillAmount = playerFuel / playerMaxFuel;
        }
    }

    private void SetPlayer()
    {
        PlayerStat n = new PlayerStat();
        JsonLoader.Save(n, "Player_Stat");
        playerStat = JsonLoader.Load<PlayerStat>("Player_Stat");
        playerInput = GetComponent<PlayerInput>();
        playerSkill = GetComponent<PlayerSkillSystem>();

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
        PlayerRotation();
    }
    private void Update()
    {
        FindTarget();
        PlayerShot();
        PlayerSkill();
        SetDelta();
    }
    private void PlayerMovement()
    {
        transform.position += playerInput.moveInput * playerSpeed * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x, Utils.moveLimit.x),
            0f, Mathf.Clamp(transform.position.z, -Utils.moveLimit.y, Utils.moveLimit.y));
    }
    private void PlayerRotation()
    {
        if(target == null)
        {
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.Euler(0, 0, 0), 0.2f);
        }
        else
        {
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetDir, 0.2f);
        }
    }
    private void FindTarget()
    {
        target = EnemySubject.Instance.NearToTargetEnemy(transform.position);

        if (target != null)
        {
            Vector3 dis = target.transform.position - transform.position;
            Quaternion LookRotation = Quaternion.LookRotation(dis);

            targetDir = LookRotation;
        }
    }
    private void PlayerShot()
    {
        if (playerInput.playerShot && shotCurTime >= shotWaitTime)
        {
            shotCurTime = 0;

            if (target == null) Shot();
            else Shot(targetDir);
        }
    }
    private void PlayerSkill()
    {
        UIManager.Instance.Skill_1Update(skill_1CurTime / playerSkill_1Cool);
        UIManager.Instance.Skill_2Update(skill_2CurTime / playerSkill_2Cool);

        if (playerInput.playerSkill1)
        {
            if(skill_1CurTime >= playerSkill_1Cool)
            {
                skill_1CurTime = 0;
                BulletSubject.Instance.EnemyBulletReflect();
                UIManager.Instance.FlahsEffect(1f);
                return;
            }
            UIManager.Instance.SkillBlock();
        }
        else if (playerInput.playerSkill2)
        {
            if(skill_2CurTime >= playerSkill_2Cool)
            {
                skill_2CurTime = 0;
                HP += 25;
                return;
            }
            UIManager.Instance.SkillBlock();
        }
    }
    private void SetDelta()
    {
        shotCurTime += Time.deltaTime;
        hitCurTime += Time.deltaTime;
        skill_1CurTime += Time.deltaTime;
        skill_2CurTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvis) return;
        if (other.CompareTag("EnemyBullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            if (hitCurTime >= hitWaitTime)
            {
                HP -= bullet.damage;
                hitCurTime = 0;

                bullet.isHit = true;
                UIManager.Instance.PlayerHitEffect(hitWaitTime);
            }
        }
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
