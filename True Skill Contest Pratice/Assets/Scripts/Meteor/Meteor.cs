using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    [SerializeField] private Image metoerHpBar;

    MeteorStat meteorStat;

    private float metoerMaxHp;
    private float hp;
    private float speed;
    private float score;
    private int rotateDir;

    public bool isDestroy { get; private set; }

    public float Hp
    {
        get => hp;
        set
        {
            hp = value;

            if (hp <= 0) isDestroy = true;
            metoerHpBar.fillAmount = hp / metoerMaxHp;
        }
    }

    public void Awake()
    {
        meteorStat = JsonLoader.Load<MeteorStat>("Metoer_Stat");

        this.hp = meteorStat.hp;
        this.speed = meteorStat.speed;
        this.score = meteorStat.score;
        metoerMaxHp = hp;

        this.rotateDir = Random.Range(-1, 2);
        if (rotateDir == 0) rotateDir = 1;
    }

    public void MeteorUpdate()
    {
        var rotateSpeed = (speed * 0.5f) * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        transform.Rotate(0, rotateSpeed * rotateDir, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Hp -= other.GetComponent<Bullet>().damage;
        }
    }
}

public class MeteorStat
{
    public float hp;
    public float speed;
    public float score;

    public MeteorStat()
    {
        hp = 50;
        speed = 45;
        score = 30;
    }
}   
