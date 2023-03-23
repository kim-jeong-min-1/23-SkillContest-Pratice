using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillSystem : Singleton<PlayerSkillSystem>
{
    private readonly float activeSkill_1CoolTime;
    private readonly float activeSkill_2CoolTime;
    private readonly int maxPlayerSkillLevel;

    private bool activeSkill_1On;
    private bool activeSkill_2On;

    private List<Skill> playerSkills;

    public PlayerSkillSystem()
    {
        activeSkill_1CoolTime = 8f;
        activeSkill_2CoolTime = 15f;
        maxPlayerSkillLevel = 4;

        activeSkill_1On = true;
        activeSkill_2On = true;

        playerSkills = new List<Skill>();
    }

    private void Awake() => SetInstance();

    private void Update()
    {
        SkillDeltaTime();
        UseSkill();
    }

    public void SelectSkill()
    {
        StartCoroutine(SelectSkill());

        IEnumerator SelectSkill()
        {
            PlayerController.Instance.enabled = false;
            Time.timeScale = 0.3f;
            int index = 0;
            int count = UIManager.Instance.selectUIs.Count;
            List<Skill> skills = new List<Skill>();

            for (int i = 0; i < count; i++)
            {
                var skill = CreateSkill();
                var level = (GetSkill(skill.type) == null) ? skill.level : GetSkill(skill.type).level;
                skill.level = level;
                skills.Add(skill);
                UIManager.Instance.SetSelectUI(i, skill);
            }
            UIManager.Instance.SkillSelectOn();
            yield return new WaitForSeconds(0.65f * Time.timeScale);

            int temp = 999;
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) index--;
                else if (Input.GetKeyDown(KeyCode.RightArrow)) index++;

                if (index != temp)
                {
                    index = Mathf.Clamp(index, 0, count - 1);
                    UIManager.Instance.SelectUIUpdate(index);
                }
                temp = index;
                yield return null;
            }

            var existingSkill = GetSkill(skills[index].type);
            if (existingSkill == null)
            {
                skills[index].level++;
                playerSkills.Add(skills[index]);
            }
            else if (existingSkill.level < maxPlayerSkillLevel) existingSkill.level++;
            //점수 추가

            //액티브 스킬은 바로 사용
            if (skills[index].isAcive) skills[index].UseSkill();

            Time.timeScale = 1f;
            PlayerController.Instance.enabled = true;
            UIManager.Instance.SkillSelectUIOff();
            yield break;
        }
    }
    public Skill CreateSkill()
    {
        Skill skill = null;
        int randSkill = Random.Range(0, 11);

        switch ((SkillType)randSkill)
        {
            case SkillType.HpUP: skill = new HpUP(); break;
            case SkillType.Invis: skill = new Invis(); break;
            case SkillType.BulletUP: skill = new BulletUP(); break;
            case SkillType.CircleBullet: skill = new CircleBullet(); break;
            case SkillType.OneLapBullet: skill = new OneLapBullet(); break;
            case SkillType.Rayzer: skill = new Rayzer(); break;
            default: skill = new FuelUP(); break;
        }
        return skill;
    }
    private void UseSkill()
    {
        if (playerSkills.Count <= 0) return;
        for (int i = 0; i < playerSkills.Count; i++)
        {
            var skill = playerSkills[i];
            if (skill.coolTime <= skill.curTime && !skill.isAcive)
            {
                skill.UseSkill();
                skill.curTime = 0;
            }
        }
    }
    private void SkillDeltaTime()
    {
        if (playerSkills.Count <= 0) return;

        for (int i = 0; i < playerSkills.Count; i++)
        {
            if (playerSkills[i].isAcive) continue;
            playerSkills[i].curTime += Time.deltaTime;
        }
    }
    private Skill GetSkill(SkillType type)
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
