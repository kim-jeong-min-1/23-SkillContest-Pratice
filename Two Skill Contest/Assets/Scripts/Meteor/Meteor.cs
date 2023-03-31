using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    [SerializeField] private Image meteorHpBar;
    private MeteorStat meteorStat;

    private float speed;
    private float meteorMaxHp;
    private float hp;
    private int roatatDir;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            meteorHpBar.fillAmount = hp / meteorMaxHp;
            if (hp <= 0) isDestroy = true;
        }
    }
    public bool isDestroy { get; private set; }
    public int score { get; private set; }

    private void Awake()
    {
        SetMeteor();
    }

    private void SetMeteor()
    {
        meteorStat = JsonLoader.Load<MeteorStat>("Meteor_Stat");

        this.speed = meteorStat.speed;
        this.hp = meteorStat.hp;
        this.score = meteorStat.score;
        meteorMaxHp = hp;

        roatatDir = Random.Range(-1, 2);
        if (roatatDir == 0) roatatDir = 1;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        transform.Rotate(new Vector3(0, roatatDir * speed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var bullet = other.GetComponent<Bullet>();

            if (bullet)
            {
                HP -= bullet.damage;
                bullet.isHit = true;
            }
        }
    }
}

[System.Serializable]
public class MeteorStat
{
    public float speed;
    public float hp;
    public int score;

    public MeteorStat()
    {
        speed = 10;
        hp = 75;
        score = 80;
    }
}
