using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Image enemyHpBar;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float enemyHp;
    private float enemyMaxHp;

    private EnemyStat enemyStat;
    private BulletShooter enemyShooter;
    private Transform player;
    public int enemyScore { get; private set; }

    public float HP
    {
        get => enemyHp;
        set
        {
            enemyHp = value;
            enemyHpBar.fillAmount = enemyHp / enemyMaxHp;

            if (enemyHp <= 0)
            {
                isDie = true;
            }
        }
    }
    public bool isDie { get; private set; } = false;


    private void Awake() => SetEnemy();
    private void Start() => StartCoroutine(EnemyAI_Update());

    private void SetEnemy()
    {
        EnemyStat n = new EnemyStat();
        JsonLoader.Save(n, "Enemy_Stat");
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        enemyShooter = GetComponent<BulletShooter>();
        player = FindObjectOfType<PlayerController>().transform;

        enemyHp = enemyStat.hp;
        enemySpeed = enemyStat.speed;
        enemyScore = enemyStat.score;

        enemyMaxHp = enemyHp;
    }

    protected abstract IEnumerator EnemyAI_Update();

    protected IEnumerator MoveToPlayerPos(float nx, float nz, float time)
    {
        float cur = 0;
        float per = 0;
        Vector3 start = transform.position;
        Vector3 end = player.position + new Vector3(nx, 0, nz) ;

        while (per < 1)
        {
            cur += Time.deltaTime;
            per = cur / time;

            transform.position = Vector3.Lerp(start, end, per);
            yield return null;  
        }
        yield break;
    }

    protected Bullet InstantiateBullet(Quaternion rot = default)
    {
        if(rot == default)
        {
            enemyShooter.Fire();
        }
        else
        {
            enemyShooter.Fire(rot);
        }
        return enemyShooter.curFireBullet;
    }

    private void GetDamage(float damage)
    {
        HP -= damage;
        HitEffect(0.1f);
    }


    private void HitEffect(float time)
    {
        StartCoroutine(HitEffect());
        IEnumerator HitEffect()
        {
            Color temp = sprite.color;
            temp.a = 150 / 255f;

            sprite.color = temp;

            yield return new WaitForSeconds(time);

            temp.a = 1;
            sprite.color = temp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet != null && bullet.type == BulletType.Player)
        {
            GetDamage(bullet.damage);
            bullet.isHit = true;
        }
    }

}

[System.Serializable]
public class EnemyStat
{
    public float hp;
    public float speed;
    public int score;

    public EnemyStat()
    {
        hp = 50;
        speed = 15;
        score = 50;
    }
}
