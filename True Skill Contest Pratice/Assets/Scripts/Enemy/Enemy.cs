using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float enemySpeed;
    [SerializeField] private string enemyName;

    [SerializeField] private Image enemyHpImage;
    [SerializeField] private SpriteRenderer sprite;

    private float maxEnemyHp;
    protected EnemyStat enemyStat;
    protected BulletShooter shooter;
    protected Transform player;

    public int score { get; private set; }
    public bool isDie { get; private set; }

    protected virtual void SetEnemy()
    {
        EnemyStat n = new EnemyStat();
        JsonLoader.Save(n, "Enemy_Stat");
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        player = FindObjectOfType<PlayerController>().transform;
        shooter = GetComponent<BulletShooter>();     

        this.enemyHp = enemyStat.hp;
        this.enemySpeed = enemyStat.speed;
        this.score = enemyStat.score;      
    }

    protected virtual void Awake() => SetEnemy();
    protected virtual void Start()
    {
        HpMult(GameManager.Instance.curStageNum);
        StartCoroutine(EnemyAI_Update());
    }
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
    public void GetDamage(float damage)
    {
        if(enemyHp - damage <= 0)
        {
            isDie = true;
        }
        enemyHp -= damage; 
        enemyHpImage.fillAmount = enemyHp / maxEnemyHp;
        StartCoroutine(HitEffect());
    }

    private void HpMult(float mult)
    {
        enemyHp *= mult;
        this.maxEnemyHp = enemyStat.hp;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();

        if (bullet && (bullet.type == BulletType.Player || bullet.type == BulletType.ChangePlayer))
        {
            bullet.isHit = true;
            GetDamage(bullet.damage);
        }
    }
}

public class EnemyStat
{
    public float hp;
    public float speed;
    public int score;

    public EnemyStat() 
    {
        hp = 50f;
        speed = 40f;
        score = 50;
    }
}

