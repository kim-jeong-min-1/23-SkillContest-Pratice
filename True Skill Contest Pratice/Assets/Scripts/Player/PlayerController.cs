using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : Singleton<PlayerController>
{
    private PlayerInput playerInput;
    private PlayerSkill playerSkill;
    private Transform model;

    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image fuelBar;

    [Space(20f)]
    [SerializeField] private GameObject rayzer;
    [SerializeField] private GameObject shield;

    private float playerHp;
    private float playerMaxHp;
    private float playerFuel;
    private float playerMaxFuel;
    private float playerSpeed;
    private float rotaionSpeed = 0.25f;

    private float shotCurTime = 0f;
    private float hitCurTime = 0f;
    private float playerHitDelayTime = 1f;
    private bool isInvis = false;
    private Quaternion targetDir;
    public Transform target { get; private set; }

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
        playerSkill = GetComponent<PlayerSkill>();
        shooters = new List<BulletShooter>();
        model = gameObject.transform.Find("model").transform;

        playerHp = playerStat.hp;
        playerFuel = playerStat.fuel;
        playerSpeed = playerStat.speed;

        playerMaxHp = playerHp;
        playerMaxFuel = playerFuel;
        ShooterLevel++;
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
        PlayerSkill();
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
        if (target != null)
        {
            Quaternion targetRot = Quaternion.Slerp(model.rotation, targetDir, rotaionSpeed);
            model.rotation = targetRot;
        }
        else
        {
            Quaternion targetRot = Quaternion.Slerp(model.rotation, Quaternion.Euler(0, 0, 0), rotaionSpeed);
            model.rotation = targetRot;
        }
    }
    private void FindToTarget()
    {
        if (target != null)
        {
            var dis = target.position - transform.position;
            dis.y = 0;

            targetDir = Quaternion.LookRotation(dis);
            UIManager.Instance.TargetSightUpdate(target.position);
        }
        else
        {
            target = EnemySubject.Instance.NearToTargetEnemy(transform.position);

            if (target == null && GameManager.Instance.curStageBoss)
                target = GameManager.Instance.curStageBoss.transform;
        }
    }
    private void PlayerShot()
    {
        if (!playerInput.playerShot || shotCurTime < shotCool) return;
        shotCurTime = 0f;

        if (target != null) ShotBullet(targetDir);
        else ShotBullet();
    }
    private void PlayerSkill()
    {
        if (playerInput.playerSkill_1) playerSkill.ActiveSkill_1();
        if (playerInput.playerSkill_2) playerSkill.ActiveSkill_2();
    }
    private void SetDeltaTime()
    {
        shotCurTime += Time.deltaTime;
        hitCurTime += Time.deltaTime;
    }
    public IEnumerator PlayerInvinCible(float time)
    {
        float curTime = 0f;
        isInvis = true;

        while (curTime < time)
        {
            curTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isInvis = false;
    }
    public IEnumerator PlayerRayzer(float time)
    {
        float curTime = 0f;
        rayzer.SetActive(true);

        while (curTime < time * 0.3f)
        {
            curTime += Time.deltaTime;
            rayzer.transform.localScale = new Vector3(1.5f * curTime, 1, 1);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(time * 0.4f);

        curTime = 0f;
        while (curTime < time * 0.3f)
        {
            curTime += Time.deltaTime;
            rayzer.transform.localScale = new Vector3(1f - 1.5f * curTime, 1, 1);

            yield return new WaitForFixedUpdate();
        }
        rayzer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvis) return;

        var bullet = other.GetComponent<Bullet>();
        if (bullet && bullet.type == BulletType.Enemy)
        {
            if (hitCurTime >= playerHitDelayTime)
            {
                hitCurTime = 0f;
                Hp -= bullet.damage;
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
