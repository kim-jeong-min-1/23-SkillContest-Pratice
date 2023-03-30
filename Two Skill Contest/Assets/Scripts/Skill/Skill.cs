using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    HpUP,
    FuelUP,
    Invincible,
    Rayzer,
    Circle,
    OneLap
}

public abstract class Skill
{
    public int level { get; set; } = 0;
    public SkillType type { get; set; }

    public float coolTime;
    public float curTime = 0;
    public string skillName;
    public string explain;

    public abstract void UseSkill();
}

public class HpUP : Skill
{
    public HpUP()
    {
        type = SkillType.HpUP;
        coolTime = 15f;
        skillName = "Hp UP";
        explain = "일정 시간 마다 내구도를 회복합니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.HP += 15 * level;
    }
}
public class FuelUP : Skill
{
    public FuelUP()
    {
        type = SkillType.FuelUP;
        coolTime = 7f;
        skillName = "Fuel UP";
        explain = "일정 시간 마다 연료를 회복합니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.HP += 150 * level;
    }
}
public class Invincible : Skill
{
    public Invincible()
    {
        type = SkillType.Invincible;
        coolTime = 10f;
        skillName = "Invincible";
        explain = "일정 시간 마다 무적이 됩니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(Invincible(3 + 1 * level));
        IEnumerator Invincible(float time)
        {
            var shield = PlayerController.Instance.playerShield;

            PlayerController.Instance.isInvis = true;
            shield.SetActive(true);

            yield return new WaitForSeconds(time);
            PlayerController.Instance.isInvis = false;
            shield.SetActive(false);
        }
    }
}
public class Rayzer : Skill
{
    public Rayzer()
    {
        type = SkillType.Rayzer;
        coolTime = 8f;
        skillName = "Rayzer";
        explain = "일정 시간 마다 강력한 레이저를 쏩니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(Rayzer(4 + 1 * level));
        IEnumerator Rayzer(float time)
        {
            var rayzer = PlayerController.Instance.playerRayzer;
            rayzer.SetActive(true);

            float cur = 0;
            while (cur < time)
            {
                cur += Time.deltaTime;

                rayzer.transform.localScale = new Vector3(4.5f * (cur / time), rayzer.transform.localScale.y, 1);
                yield return new WaitForEndOfFrame();
            }

            cur = 0;
            while (cur < time)
            {
                cur += Time.deltaTime;

                rayzer.transform.localScale = new Vector3(4.5f - 4.5f * (cur / time), rayzer.transform.localScale.y, 1);
                yield return new WaitForEndOfFrame();
            }
            rayzer.SetActive(false);
        }
    }
}
public class Circle : Skill
{
    public Circle()
    {
        type = SkillType.Circle;
        coolTime = 6f;
        skillName = "Circle";
        explain = "일정 시간마다 원형탄을 쏩니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(Circle());
        IEnumerator Circle()
        {
            for (int j = 0; j < level; j++)
            {
                for (int i = 0; i < 360; i += 360 / 20)
                {
                    Bullet n = BulletPool.Instance.GetBullet();
                    n.SetBullet(45, 25, Quaternion.Euler(0, i, 0), EntityType.Player);
                    n.transform.position = PlayerController.Instance.transform.position;
                    BulletSubject.Instance.AddBullet(n);              
                }
                yield return new WaitForSeconds(0.2f);
            }            
        }
    }
}
public class OneLap : Skill
{
    public OneLap()
    {
        type = SkillType.OneLap;
        coolTime = 6f;
        skillName = "OneLap";
        explain = "일정 시간 마다 총알을 한 바퀴 발사합니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(OneLap());
        IEnumerator OneLap()
        {
            int count = 18 + 2 * level;
            for (int i = 0; i < 360; i += 360 / count)
            {
                PlayerController.Instance.Shot(Quaternion.Euler(0, i, 0));
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}

