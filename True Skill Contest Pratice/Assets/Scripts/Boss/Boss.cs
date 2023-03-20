using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : MonoBehaviour
{
    [SerializeField] protected float bossHp;
    [SerializeField] protected float bossSpeed;
    [SerializeField] private float score;

    [SerializeField] private Image bossHpImage;
    [SerializeField] private SpriteRenderer sprite;

    private float maxBossHp;
    protected BossStat bossStat;
    protected BulletShooter shooter;
    protected Transform player;

    public bool isDie { get; private set; }

    protected virtual void SetBoss()
    {
        BossStat stat = new BossStat();
        JsonLoader.Save(stat, "Boss_Stat");
        bossStat = JsonLoader.Load<BossStat>("Boss_Stat");
        player = FindObjectOfType<PlayerController>().transform;
        shooter = GetComponent<BulletShooter>();

        this.bossHp = bossStat.hp;
        this.maxBossHp = bossStat.hp;
        this.bossSpeed = bossStat.speed;
        this.score = bossStat.score;
    }

    protected virtual void Awake() => SetBoss();
    protected abstract IEnumerator BossAI_Update();
    protected IEnumerator MoveToPlayerPosition(float nX, float nZ, float time)
    {
        Vector3 Pos = player.position;
        Pos = new Vector3(Pos.x + nX, Pos.y, Pos.z + nZ);

        float current = 0;
        float percent = 0;
        Vector3 startPos = transform.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(startPos, Pos, percent);
            yield return new WaitForEndOfFrame();
        }
    }
    protected IEnumerator MoveTo(Vector3 targetPos, float time)
    {
        float current = 0;
        float percent = 0;
        Vector3 startPos = transform.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(startPos, targetPos, percent);
            yield return new WaitForEndOfFrame();
        }
    }
    protected Bullet InstantiateBullet(Quaternion rot = default)
    {
        if (rot != default)
        {
            shooter.fire(rot);
        }
        else
        {
            shooter.fire();
        }
        return shooter.curFireBullet;
    }
    private IEnumerator HitEffect()
    {
        Color tempColor = sprite.color;
        tempColor.a = 160f / 255;
        sprite.color = tempColor;
        yield return new WaitForSeconds(0.08f);

        tempColor.a = 1;
        sprite.color = tempColor;
        yield break;
    }
    private void GetDamage(float damage)
    {
        if (bossHp - damage <= 0)
        {
            isDie = true;
        }
        bossHp -= damage;
        bossHpImage.fillAmount = bossHp / maxBossHp;

        StartCoroutine(HitEffect());
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var bullet = other.GetComponent<Bullet>();

            bullet.isHit = true;
            GetDamage(bullet.damage);
        }
    }
}

public class BossStat
{
    public float hp;
    public float speed;
    public float score;

    public BossStat()
    {
        hp = 5000f;
        speed = 30f;
        score = 200;
    }
}