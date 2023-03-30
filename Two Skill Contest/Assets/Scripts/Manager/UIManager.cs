using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image flashUI;
    [SerializeField] private Image skill_1UI;
    [SerializeField] private Image skill_2UI;
    [SerializeField] private GameObject skillBlockUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text questText;

    private void Awake() => SetInstance();

    private void Update()
    {
        questText.text = $"Enemy {EnemySubject.Instance.enemyCount} / {GameManager.Instance.quest}";
        scoreText.text = $"Score : {GameManager.Instance.score}";
    }

    public void Skill_1Update(float value) => skill_1UI.fillAmount = value;
    public void Skill_2Update(float value) => skill_2UI.fillAmount = value;
    public void SkillBlock()
    {
        StartCoroutine(SkillBlock());
        IEnumerator SkillBlock()
        {
            skillBlockUI.SetActive(true);

            yield return new WaitForSeconds(0.25f);

            skillBlockUI.SetActive(false);
        }
    }
    public void PlayerHitEffect(float time)
    {
        StartCoroutine(PlayerHitEffect());
        IEnumerator PlayerHitEffect()
        {
            float cur = 0;
            float per = 0;
            Color temp = playerHitUI.color;

            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.7f);

                temp.a = Mathf.Lerp(0, 150 / 255f, per);
                playerHitUI.color = temp;
                yield return null;
            }

            cur = 0;
            per = 0;
            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.3f);

                temp.a = Mathf.Lerp(150 / 255f, 0, per);
                playerHitUI.color = temp;
                yield return null;
            }
        }
    }
    public void FlahsEffect(float time)
    {
        StartCoroutine(FlashEffect());
        IEnumerator FlashEffect()
        {
            float cur = 0;
            float per = 0;
            Color temp = flashUI.color;

            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.4f);

                temp.a = Mathf.Lerp(0, 250 / 255f, per);
                flashUI.color = temp;
                yield return null;
            }

            cur = 0;
            per = 0;
            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.6f);

                temp.a = Mathf.Lerp(250 / 255f, 0, per);
                flashUI.color = temp;
                yield return null;
            }
        }
    }
}
