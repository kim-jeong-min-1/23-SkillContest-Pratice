using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStat enemyStat;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Image enemyHpBar;
    private BulletShooter enemyShooter;

    private float enemyMaxHp;
    private float enemyHp;
    protected float enemySpeed;
    public int score { get; set; }
    public float HP
    {
        get => enemyHp;
        set
        {
            enemyHp = value;
            if (enemyHp <= 0) isDie = true;

            HitEffect(0.1f);
            enemyHpBar.fillAmount = enemyHp / enemyMaxHp;
        }
    }
    public bool isDie { get; set; } = false;

    private void Awake() => SetEnemy();
    private void Start() => StartCoroutine(Enemy_AI());

    private void SetEnemy()
    {
        EnemyStat n = new EnemyStat();
        JsonLoader.Save(n, "Enemy_Stat");
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        enemyShooter = GetComponent<BulletShooter>();

        enemyHp = enemyStat.hp;
        enemySpeed = enemyStat.speed;
        score = enemyStat.score;

        enemyMaxHp = enemyHp;
    }

    protected abstract IEnumerator Enemy_AI();
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
        if (rot == default) enemyShooter.Fire();
        else enemyShooter.Fire(rot);
        
        return enemyShooter.curFireBullet;
    }
    private void HitEffect(float time)
    {
        StartCoroutine(HitEffect());
        IEnumerator HitEffect()
        {
            Color tempColor = sprite.color;
            tempColor.a = 150 / 255f;

            sprite.color = tempColor;
            yield return new WaitForSeconds(time);

            tempColor.a = 1;
            sprite.color = tempColor;
        }
    }
    public void HpMult(int mult)
    {
        enemyHp *= mult;
        enemyMaxHp = enemyHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            HP -= bullet.damage;

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
public class EnemyStat
{
    public float speed;
    public float hp;
    public int score;

    public EnemyStat()
    {
        speed = 15f;
        hp = 50f;
        score = 50;
    }
}