using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : Singleton<PlayerController>
{
    public PlayerSkillSystem playerSkill { get; private set; }
    private PlayerInput playerInput;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Image playerHpBar;
    [SerializeField] private Image playerFuelBar;
    public GameObject shield;
    public GameObject rayzer;

    private float playerSpeed;
    private float playerMaxHp;
    private float playerHp;
    private float playerMaxFuel;
    private float playerFuel;

    private float shotCurTime;
    private float skill_1CurTime;
    private float skill_2CurTime;
    private float hitCurTime = 0;

    private readonly float shotWaitTime = 0.15f;
    private readonly float hitWaitTime = 1f;
    private readonly float rotationSpeed = 0.2f;
    private readonly float skill_1Cool = 15f;
    private readonly float skill_2Cool = 8f;

    private Transform target;
    private Quaternion targetDir;

    public bool isInvis { get; set; } = false;
    public bool isDie { get; set; } = false;

    public float HP
    {
        get => playerHp;
        set
        {
            playerHp = value;
            if (playerHp <= 0) PlayerDie();
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
            if (playerFuel <= 0) PlayerDie();
            else if (playerFuel > playerMaxFuel) playerFuel = playerMaxFuel;

            playerFuelBar.fillAmount = playerFuel / playerMaxFuel;
        }
    }

    private void Awake()
    {
        SetInstance();
        SetPlayer();
    }

    private void SetPlayer()
    {
        playerStat = JsonLoader.Load<PlayerStat>("Player_Stat");
        playerInput = GetComponent<PlayerInput>();
        playerSkill = GetComponent<PlayerSkillSystem>();

        playerSpeed = playerStat.speed;
        playerHp = playerStat.hp;
        playerFuel = playerStat.fuel;

        playerMaxHp = playerHp;
        playerMaxFuel = playerFuel;

        ShooterLevel++;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
        FindToTarget();
    }
    private void Update()
    {
        SetDelta();       
        PlayerShot();
        PlayerSkill();
    }

    private void PlayerMovement()
    {
        transform.position += playerInput.moveInput * playerSpeed * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Utils.moveLimit.x, Utils.moveLimit.x), 0f,
            Mathf.Clamp(transform.position.z, -Utils.moveLimit.y, Utils.moveLimit.y));
    }
    private void PlayerRotation()
    {
        if (target != null)
        {
            var dis = target.position - transform.position;
            dis.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dis);
            targetDir = Quaternion.Slerp(playerObj.transform.rotation, targetRotation, rotationSpeed);
        }
        else
        {
            targetDir = Quaternion.Slerp(playerObj.transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed);
        }
        playerObj.transform.rotation = targetDir;
    }
    private void FindToTarget()
    {
        target = EnemySubject.Instance.NearToTargetEnemy(transform.position);

        if (target == null && GameManager.Instance.curStageBoss != null)
        {
            target = GameManager.Instance.curStageBoss.transform;
        }

        if(target != null)
        {
            var target = Camera.main.WorldToScreenPoint(this.target.position);
            UIManager.Instance.TargetSightUpdate(target);
        }
    }
    private void PlayerShot()
    {
        if (playerInput.playerShot && shotCurTime >= shotWaitTime)
        {
            shotCurTime = 0;           
            if (target == null) ShotBullet();
            else ShotBullet(targetDir);
            SoundManager.Instance.PlaySfx(SoundEffect.Shot, 0.35f);
        }
    }
    private void PlayerSkill()
    {
        if (playerInput.playerSkill_1)
        {
            if (skill_1CurTime >= skill_1Cool)
            {
                skill_1CurTime = 0;
                HP += 25;
                SoundManager.Instance.PlaySfx(SoundEffect.Skill1, 1f);
                return;
            }
            UIManager.Instance.SkillBlock(0.1f);
        }

        if (playerInput.playerSkill_2)
        {
            if (skill_2CurTime >= skill_2Cool)
            {
                skill_2CurTime = 0;
                BulletSubject.Instance.ChangeToEnemyBullet();
                UIManager.Instance.FlashEffect(0.8f);
                SoundManager.Instance.PlaySfx(SoundEffect.Skill2, 1f);
                return;
            }
            UIManager.Instance.SkillBlock(0.1f);
        }
    }
    private void SetDelta()
    {
        shotCurTime += Time.deltaTime;
        hitCurTime += Time.deltaTime;
        skill_1CurTime += Time.deltaTime;
        skill_2CurTime += Time.deltaTime;

        Fuel -= Time.deltaTime * 7f;
        UIManager.Instance.SetSkill_1UI(skill_1CurTime / skill_1Cool);
        UIManager.Instance.SetSkill_2UI(skill_2CurTime / skill_2Cool);
    }

    private void PlayerDie()
    {
        if (!isDie)
        {
            isDie = true;
            GameManager.Instance.GameOver();
        }
    }

    public void PlayerReset()
    {
        StopAllCoroutines();
        transform.position = new Vector3(0, 0, -50f);
        playerObj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //쿨타임 초기화 치트키
    public void CoolTimeReset()
    {
        skill_1CurTime = skill_1Cool;
        skill_2CurTime = skill_2Cool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvis) return;

        if (other.CompareTag("EnemyBullet") && hitCurTime >= hitWaitTime)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet.isReflect || bullet.isHit) return;

            hitCurTime = 0;
            HP -= bullet.damage;
            bullet.isHit = true;
            UIManager.Instance.PlayerHitEffect(hitWaitTime / 2f);
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
        speed = 20;
        fuel = 1000;
    }
}
