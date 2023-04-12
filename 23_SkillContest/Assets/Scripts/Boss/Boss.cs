using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : MonoBehaviour
{
    [SerializeField] private BossStat bossStat;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Image bossHpBar;
    private BulletShooter bossShooter;

    private float bossMaxHp;
    private float bossHp;
    protected float bossSpeed;
    public int score { get; set; }
    public float HP
    {
        get => bossHp;
        set
        {
            bossHp = value;
            if (bossHp <= 0)
            {
                isDie = true;
                StopAllCoroutines();
            }
            HitEffect(0.1f);
            bossHpBar.fillAmount = bossHp / bossMaxHp;
        }
    }
    public bool isDie { get; set; } = false;

    private void Awake() => SetEnemy();
    private void Start() => StartCoroutine(Boss_AI());

    private void SetEnemy()
    {
        BossStat n = new BossStat();
        JsonLoader.Save(n, "Boss_Stat");
        bossStat = JsonLoader.Load<BossStat>("Boss_Stat");
        bossShooter = GetComponent<BulletShooter>();

        bossHp = bossStat.hp;
        bossSpeed = bossStat.speed;
        score = bossStat.score;

        bossMaxHp = bossHp;
    }

    protected abstract IEnumerator Boss_AI();
    protected IEnumerator MoveToPlayerPos(float nx, float nz, float time)
    {
        float cur = 0;
        float per = 0;

        Vector3 start = transform.position;
        Vector3 end = PlayerController.Instance.transform.position + new Vector3(nx, 0f, nz);

        while (per < 1)
        {
            cur += Time.deltaTime;
            per = cur / time;

            transform.position = Vector3.Lerp(start, end, per);
            yield return null;
        }
    }
    protected Bullet InstantiateBullet(Quaternion rot = default)
    {
        if (rot == default) bossShooter.Fire();
        else bossShooter.Fire(rot);

        return bossShooter.curFireBullet;
    }
    private void HitEffect(float time)
    {
        StartCoroutine(HitEffect());
        IEnumerator HitEffect()
        {
            Color tempColor = sprite.color;
            tempColor.a = 220 / 255f;

            sprite.color = tempColor;
            yield return new WaitForSeconds(time);

            tempColor.a = 1;
            sprite.color = tempColor;
        }
    }
    public void HpMult(int mult)
    {
        bossHp *= mult;
        bossMaxHp = bossHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var bullet = other.GetComponent<Bullet>();

            if (bullet.isReflect) HP -= (bullet.damage / 3f);
            else HP -= bullet.damage;

            bullet.isHit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerRayzer"))
        {
            HP -= 2f;
        }
    }
}

[System.Serializable]
public class BossStat
{
    public float speed;
    public float hp;
    public int score;

    public BossStat()
    {
        speed = 12f;
        hp = 5000f;
        score = 200;
    }
}
