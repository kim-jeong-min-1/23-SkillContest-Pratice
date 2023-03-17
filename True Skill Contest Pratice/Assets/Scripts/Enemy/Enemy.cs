using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyHp;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float exp;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Image enemyHpImage;

    protected EnemyStat enemyStat;
    protected BulletShooter shooter;
    protected Transform player;

    protected virtual void SetEnemy()
    {
        enemyStat = JsonLoader.Load<EnemyStat>("Enemy_Stat");
        player = FindObjectOfType<PlayerController>().transform;
        shooter = GetComponent<BulletShooter>();     

        this.enemyHp = enemyStat.hp;
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

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(transform.position, Pos, percent);
            yield return null;
        }
    }
    protected IEnumerator MoveTo(Vector3 targetPos, float time)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(transform.position, targetPos, percent);
            yield return null;
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

    private void GetDamage(float damage)
    {
        if(enemyHp - damage <= 0)
        {
            Die(); return;
        }

        var lerp = Mathf.Lerp(0f, enemyHp, enemyHp - damage);
        enemyHpImage.fillAmount = lerp;
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
        speed = 10f;
        exp = 15f;
    }
}

