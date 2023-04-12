using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    HpUP,
    FuelUP,
    BulletUP,
    Invincible,
    Circle,
    OneLap,
    Rayzer
}

public abstract class Skill
{
    public int level { get; set; } = 0;
    public SkillType type { get; set; }

    public float cool;
    public float curTime = 0;
    public string name;
    public string explain;
    public bool isActive;

    public abstract void UseSkill();
}

public class HpUP : Skill
{
    public HpUP()
    {
        type = SkillType.HpUP;
        cool = 8f;
        name = "HP UP";
        explain = "일정 시간 마다 내구도를 수리합니다.";
        isActive = true;
    }

    public override void UseSkill()
    {
        PlayerController.Instance.HP += level * 25f;
    }
}

public class FuelUP : Skill
{
    public FuelUP()
    {
        type = SkillType.FuelUP;
        cool = 8f;
        name = "Fuel UP";
        explain = "일정 시간 마다 연료를 회복합니다.";
        isActive = true; 
    }

    public override void UseSkill()
    {
        PlayerController.Instance.Fuel += 200 * level;
    }
}

public class BulletUP : Skill
{
    public BulletUP()
    {
        type = SkillType.BulletUP;
        level = 1;
        name = "Bullet UP";
        explain = "플레이어 공격을 업그레이드 합니다.";
        isActive = true; 
    }

    public override void UseSkill()
    {
        PlayerController.Instance.ShooterLevel++;
    }
}

public class Invincible : Skill
{
    public Invincible()
    {
        type = SkillType.Invincible;
        cool = 12f;
        name = "Invincible";
        explain = "일정 시간 마다 무적이 됩니다.";
        isActive = true;
    }
    private Coroutine preInvincible = null;

    public override void UseSkill()
    {
        if(preInvincible != null) PlayerController.Instance.StopCoroutine(preInvincible);
        preInvincible = PlayerController.Instance.StartCoroutine(Invincible());

        IEnumerator Invincible()
        {
            var shield = PlayerController.Instance.shield;

            shield.SetActive(true);
            PlayerController.Instance.isInvis = true;
            yield return new WaitForSeconds(2 + 1 * level);

            shield.SetActive(false);
            PlayerController.Instance.isInvis = false;
        }
    }
}

public class Circle : Skill
{
    public Circle()
    {
        type = SkillType.Circle;
        cool = 6f;
        name = "Circle";
        explain = "일정 시간 마다 원형탄을 발사 합니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(Circle());
        IEnumerator Circle()
        {
            for (int i = 0; i < level; i++)
            {
                for (int k = 0; k < 360; k += 360 / 20)
                {
                    Bullet bullet = BulletPool.Instance.GetPlayerBullet();
                    bullet.SetBullet(30, 25, Quaternion.Euler(0, k, 0), EntityType.Player);
                    bullet.transform.position = PlayerController.Instance.transform.position;
                    BulletSubject.Instance.AddBullet(bullet);
                }
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}

public class OneLap : Skill
{
    public OneLap()
    {
        type = SkillType.OneLap;
        cool = 6f;
        name = "OneLap";
        explain = "일정 시간 마다 총알을 한 바퀴 발사 합니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(OneLap());
        IEnumerator OneLap()
        {
            for (int i = 0; i < 360; i += 360 / (18 + 2 * level))
            {
                PlayerController.Instance.ShotBullet(Quaternion.Euler(0, i, 0));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

public class Rayzer : Skill
{
    public Rayzer()
    {
        type = SkillType.Rayzer;
        cool = 7f;
        name = "Rayzer";
        explain = "일정 시간 마다 레이저를 쏩니다.";
    }

    public override void UseSkill()
    {
        PlayerController.Instance.StartCoroutine(Rayzer(1 + level));
        IEnumerator Rayzer(float time)
        {
            SoundManager.Instance.PlaySfx(SoundEffect.Rayzer, 1f);
            var rayer = PlayerController.Instance.rayzer;
            rayer.SetActive(true);

            float cur = 0;
            while (cur < time)
            {
                cur += Time.deltaTime;

                rayer.transform.localScale = new Vector3(8f * (cur / time), rayer.transform.localScale.y, 1);
                yield return new WaitForEndOfFrame();
            }
            cur = 0;
            while (cur < time)
            {
                cur += Time.deltaTime;

                rayer.transform.localScale = new Vector3(8f - 8f * (cur / time), rayer.transform.localScale.y, 1);
                yield return new WaitForEndOfFrame();
            }
            rayer.SetActive(false);
        }
    }
}