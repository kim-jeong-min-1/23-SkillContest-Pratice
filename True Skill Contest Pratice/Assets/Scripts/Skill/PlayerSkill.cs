using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private readonly float activeSkill_1CoolTime = 8f;
    private readonly float activeSkill_2CoolTime = 15f;
    private bool activeSkill_1On = true;
    private bool activeSkill_2On = true;
    

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
                StartCoroutine(SkillTimer(activeSkill_1CoolTime, UIManager.Instance.Skill1_UIUpdate));

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
            PlayerController.Instance.Hp += 50f;
            activeSkill_2On = false;

            yield return
                StartCoroutine(SkillTimer(activeSkill_2CoolTime, UIManager.Instance.Skill2_UIUpdate));

            activeSkill_2On = true;
            yield break;
        }
    }
    private IEnumerator SkillTimer(float coolTime, System.Action<float> uiUpdate = null)
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
