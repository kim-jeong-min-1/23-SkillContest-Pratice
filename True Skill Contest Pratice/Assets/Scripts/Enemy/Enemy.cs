using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float enemySpeed;
    [SerializeField] private float score;
    [SerializeField] private string enemyName;

    [SerializeField] private Image enemyHpImage;
    [SerializeField] private SpriteRenderer sprite;

    private float maxEnemyHp;
    protected EnemyStat enemyStat;
    protected BulletShooter shooter;
    protected Transform player;

    public bool isDie { get; private set; }

    protected virtual void SetEnemy()
    {
        EnemyStat n = new EnemyStat();
        JsonLoader.Save(n, "Enemy_Stat");
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        player = FindObjectOfType<PlayerController>().transform;
        shooter = GetComponent<BulletShooter>();     

        this.enemyHp = enemyStat.hp;
        this.maxEnemyHp = enemyStat.hp;
        this.enemySpeed = enemyStat.speed;
        this.score = enemyStat.score;
    }

    protected virtual void Awake() => SetEnemy();
    protected abstract IEnumerator EnemyAI_Update();
    
    protected void EnemyMovement()
    {
        transform.Translate(-transform.forward * enemySpeed * Time.deltaTime);
    }
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
        if(rot != default)
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
    }
    private void GetDamage(float damage)
    {
        if(enemyHp - damage <= 0)
        {
            isDie = true;
        }
        enemyHp -= damage; 
        enemyHpImage.fillAmount = enemyHp / maxEnemyHp;
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

public class EnemyStat
{
    public float hp;
    public float speed;
    public float score;

    public EnemyStat()
    {
        hp = 100f;
        speed = 45f;
        score = 50;
    }
}

