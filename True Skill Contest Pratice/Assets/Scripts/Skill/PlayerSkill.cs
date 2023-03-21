using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private readonly float activeSkill_1CoolTime;
    private readonly float activeSkill_2CoolTime;
    private readonly int maxPlayerSkillLevel;

    private bool activeSkill_1On;
    private bool activeSkill_2On;

    private List<Skill> playerSkills;

    public PlayerSkill()
    {
        activeSkill_1CoolTime = 8f;
        activeSkill_2CoolTime = 15f;
        maxPlayerSkillLevel = 4;

        activeSkill_1On = true;
        activeSkill_2On = true;

        playerSkills = new List<Skill>();
    }

    private void Update()
    {
        SkillDeltaTime();
        UseSkill();
    }

    public void GetSkill()
    {

    }

    private void UseSkill()
    {
        for (int i = 0; i < playerSkills.Count; i++)
        {
            var skill = playerSkills[i];
            if (skill.coolTime <= skill.curTime)
            {
                if (skill.isAcive) continue;
                skill.UseSkill();
                skill.curTime = 0;
            }
        }
    }
    private void SkillDeltaTime()
    {
        if (playerSkills.Count > 0)
            for (int i = 0; i < playerSkills.Count; i++)
            {
                if (playerSkills[i].isAcive) continue;
                playerSkills[i].curTime += Time.deltaTime;
            }
    }
    private Skill ReturnSkill(SkillType type)
    {
        for (int i = 0; i < playerSkills.Count; i++)
        {
            if (type == playerSkills[i].type) return playerSkills[i];
        }
        return null;
    }

    public void ActiveSkill_1()
    {
        if (activeSkill_1On) StartCoroutine(PlayerActiveSkill1());
        else UIManager.Instance.SkillDisable();

        IEnumerator PlayerActiveSkill1()
        {
            BulletSubject.Instance.ReflectToEnemyBullet();
            UIManager.Instance.FlashEffect(0.8f);

            activeSkill_1On = false;

            yield return
                StartCoroutine(ActiveSkillTimer(activeSkill_1CoolTime, UIManager.Instance.Skill1_UIUpdate));

            activeSkill_1On = true;
            yield break;
        }
    }
    public void ActiveSkill_2()
    {
        if (activeSkill_2On) StartCoroutine(PlayerActiveSkill2());
        else UIManager.Instance.SkillDisable();

        IEnumerator PlayerActiveSkill2()
        {
            PlayerController.Instance.Hp += 25f;
            activeSkill_2On = false;

            yield return
                StartCoroutine(ActiveSkillTimer(activeSkill_2CoolTime, UIManager.Instance.Skill2_UIUpdate));

            activeSkill_2On = true;
            yield break;
        }
    }
    private IEnumerator ActiveSkillTimer(float coolTime, System.Action<float> uiUpdate = null)
    {
        float curTime = 0;

        while (curTime < coolTime)
        {
            curTime += Time.deltaTime;
            uiUpdate?.Invoke(curTime / coolTime);

            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
