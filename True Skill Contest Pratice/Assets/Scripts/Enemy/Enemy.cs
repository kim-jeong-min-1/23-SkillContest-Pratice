using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float exp;
    [SerializeField] private Image enemyHpImage;
    [SerializeField] private SpriteRenderer sprite;

    private float maxEnemyHp;
    protected EnemyStat enemyStat;
    protected BulletShooter shooter;
    protected Transform player;

    protected virtual void SetEnemy()
    {
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        player = FindObjectOfType<PlayerController>().transform;
        shooter = GetComponent<BulletShooter>();     

        this.enemyHp = enemyStat.hp;
        this.maxEnemyHp = enemyStat.hp;
        this.enemySpeed = enemyStat.speed;
        this.exp = enemyStat.exp;
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
    protected IEnumerator DefaultEnemyShooter()
    {
        while (gameObject)
        {
            shooter.fire(Quaternion.Euler(0, 180f, 0));
            yield return new WaitForSeconds(shooter.cool);
        }
    }
    protected Bullet InstantiateBullet(Quaternion rot = default, Vector3 pos = default)
    {
        if(rot != default)
        {
            shooter.fire(rot);
        }
        else if(pos != default)
        {
            shooter.fire(pos, rot);
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

        Color color = tempColor;
        color.a = 160f / 255;

        sprite.color = color;
        yield return new WaitForSeconds(0.1f);

        sprite.color = tempColor;
        yield break;
    }
    private void GetDamage(float damage)
    {
        if(enemyHp - damage <= 0)
        {
            Die(); return;
        }

        enemyHp -= damage; 
        enemyHpImage.fillAmount = enemyHp / maxEnemyHp;
        StartCoroutine(HitEffect());
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            GetDamage(other.GetComponent<Bullet>().damage);
            Destroy(other.gameObject);
        }
    }
}

public class EnemyStat
{
    public float hp;
    public float speed;
    public float exp;

    public EnemyStat()
    {
        hp = 100f;
        speed = 20f;
        exp = 15f;
    }
}

