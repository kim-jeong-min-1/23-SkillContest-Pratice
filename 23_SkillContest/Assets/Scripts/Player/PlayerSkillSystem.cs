using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
    private readonly int skillMaxLevel = 4;
    private int choice;

    private List<Skill> playerSkills = new List<Skill>();

    public void SelectSkill()
    {
        SoundManager.Instance.PlaySfx(SoundEffect.SelectApear, 1f);
        StartCoroutine(SelectSkill());
        IEnumerator SelectSkill()
        {
            Skill[] skills = new Skill[3];
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i] = SkillFactory.CreateSkill((SkillType)Random.Range(0, 10));

                if (skills[i].type == SkillType.BulletUP) skills[i].level = PlayerController.Instance.ShooterLevel;

                var exstingSkill = GetSkill(skills[i].type);
                if (exstingSkill != null) skills[i] = exstingSkill;
            }

            UIManager.Instance.SetSelectUI(skills);
            UIManager.Instance.SelectUIActive(true);
            Time.timeScale = 0.3f;
            yield return StartCoroutine(UIManager.Instance.SelectUIMove(0f));
            yield return StartCoroutine(Choice());

            if (skills[choice].level < skillMaxLevel) skills[choice].level++;
            else GameManager.Instance.Score += 200;

            if (skills[choice].level == 1) playerSkills.Add(skills[choice]);
            if(skills[choice].isActive) skills[choice].UseSkill();

            yield return StartCoroutine(UIManager.Instance.SelectUIMove(-1000f));
            UIManager.Instance.SelectUIActive(false);
            Time.timeScale = 1f;
        }      
    }

    private IEnumerator Choice()
    {
        choice = 0;
        UIManager.Instance.ChangeSelect(choice);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                choice--;
                choice = Mathf.Clamp(choice, 0, 2);
                UIManager.Instance.ChangeSelect(choice);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                choice++;
                choice = Mathf.Clamp(choice, 0, 2);
                UIManager.Instance.ChangeSelect(choice);
            }
            yield return null;
        }
        SoundManager.Instance.PlaySfx(SoundEffect.Select, 1f);
    }


    private void Update()
    {
        if (playerSkills.Count == 0) return;

        for (int i = 0; i < playerSkills.Count; i++) playerSkills[i].curTime += Time.deltaTime;
        PlayerSkillUse();
    }

    private void PlayerSkillUse()
    {
        foreach (var skill in playerSkills)
        {
            if(skill.curTime >= skill.cool)
            {
                skill.curTime = 0;
                skill.UseSkill();
            }
        }
    }

    private Skill GetSkill(SkillType type)
    {
        foreach (var skill in playerSkills)
        {
            if (skill.type == type) return skill;
        }
        return null;
    }
    
}
