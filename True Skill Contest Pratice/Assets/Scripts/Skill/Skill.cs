using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    HpUP,
    FuelUP,
    Invis,
    BulletUP,
    CircleBullet,
    Rayzer,
    OneLapBullet,
}

public abstract class Skill
{
    public SkillType type { get; set; }
    public int level { get; set; } = 1;

    public string name;
    public string explain;

    public float coolTime;
    public float curTime = 0;

    public bool isAcive = false;

    public abstract void UseSkill();
}

public class HpUP : Skill
{
    public HpUP()
    {
        type = SkillType.HpUP;
        name = "HP UP";
        explain = "���� �ð����� �������� �����մϴ�";
        coolTime = 10f;
    }

    public override void UseSkill()
    {
        PlayerController.Instance.Hp += 10 * level;
    }
}

public class FuelUP : Skill
{
    public FuelUP()
    {
        type = SkillType.FuelUP;
        name = "FUEL UP";
        explain = "���� �ð����� ���Ḧ �ο��մϴ�.";
        coolTime = 6f;
    }
    public override void UseSkill()
    {
        PlayerController.Instance.Fuel += 200 * level;
    }
}

public class Invis : Skill
{
    public Invis()
    {
        type = SkillType.Invis;
        name = "INVINCIBLE";
        explain = "���� �ð� ���� ������ �˴ϴ�.";
        coolTime = 8f;
    }
    public override void UseSkill()
    {
        PlayerController.Instance.PlayerInvincible(1.5f * level);
    }
}

public class BulletUP : Skill
{
    public BulletUP()
    {
        type = SkillType.BulletUP;
        name = "BULLET UP";
        explain = "�÷��̾��� ������ �� �ܰ� ���׷��̵� �մϴ�.";
        isAcive = true;
    }
    public override void UseSkill()
    {
        PlayerController.Instance.ShooterLevel++;
    }
}

public class CircleBullet : Skill
{
    public CircleBullet()
    {
        type = SkillType.CircleBullet;
        name = "CIRCLE";
        explain = "���� �ð����� �������� źȯ�� �߻��մϴ�.";
        coolTime = 6f;
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(CircleShot());

        IEnumerator CircleShot()
        {
            for (int i = 0; i < level; i++)
            {
                for (int j = 0; j < 360; j += 360 / 18)
                {
                    var bullet = BulletPool.Instance.GetPlayerBullet();
                    bullet.SetBullet(100, 20, Quaternion.Euler(0, j, 0));
                    bullet.transform.position = PlayerController.Instance.transform.position;
                    BulletSubject.Instance.AddBullet(bullet);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }        
    }
}

public class Rayzer : Skill
{
    public Rayzer()
    {
        type = SkillType.Rayzer;
        name = "RAYZER";
        explain = "�����ð� ���� Ÿ�ٿ��� �������� �߻� �մϴ�.";
        coolTime = 7f;
    }

    public override void UseSkill()
    {
        PlayerController.Instance.PlayerRayzerOn(3f, 1.5f * level);
    }
}

public class OneLapBullet : Skill
{
    public OneLapBullet()
    {
        type = SkillType.OneLapBullet;
        name = "ONE LAP";
        explain = "���� �ð����� �Ѿ��� �� ���� �߻��մϴ�.";
        coolTime = 6f;
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(OneLapBullet());
        IEnumerator OneLapBullet()
        {
            int count = 8 + (level * 2);
            for (int i = 0; i <= 360; i+= 360 / count)
            {
                PlayerController.Instance.ShotBullet(Quaternion.Euler(0, i, 0));
                yield return new WaitForSeconds(0.12f);
            }
        }
    }
}