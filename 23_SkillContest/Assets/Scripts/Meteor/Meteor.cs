using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    [SerializeField] private MeteorStat meteorStat;
    [SerializeField] private Image meteorHpBar;

    private float meteorMaxHp;
    private float metoerHp;
    private float metoerSpeed;
    private int rotateDir;

    public int score { get; set; }
    public float HP
    {
        get => metoerHp;
        set
        {
            metoerHp = value;
            if (metoerHp <= 0) isDestroy = true;

            meteorHpBar.fillAmount = metoerHp / meteorMaxHp;
        }
    }
    public bool isDestroy { get; set; } = false;

    private void Awake() => SetEnemy();

    private void SetEnemy()
    {
        MeteorStat n = new MeteorStat();
        JsonLoader.Save(n, "Meteor_Stat");
        meteorStat = JsonLoader.Load<MeteorStat>("Meteor_Stat");

        metoerHp = meteorStat.hp;
        metoerSpeed = meteorStat.speed;
        score = meteorStat.score;
        meteorMaxHp = metoerHp;

        rotateDir = Random.Range(-1, 1);
        if (rotateDir == 0) rotateDir = 1; 
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -metoerSpeed * Time.deltaTime));
        transform.Rotate(new Vector3(0, rotateDir * metoerSpeed * Time.deltaTime, 0));
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
}

[System.Serializable]
public class MeteorStat
{
    public float speed;
    public float hp;
    public int score;

    public MeteorStat()
    {
        speed = 18f;
        hp = 75f;
        score = 80;
    }
}
