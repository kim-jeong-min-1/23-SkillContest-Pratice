using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image skill_1UI;
    [SerializeField] private Image skill_2UI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text questText;

    private void Awake() => SetInstance();

    private void Update()
    {
        questText.text = $"Enemy {EnemySubject.Instance.enemyCount} / {GameManager.Instance.quest}";
        scoreText.text = $"Score : {GameManager.Instance.score}";
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

                temp.a = Mathf.Lerp(0, 150 / 225f, per);
                playerHitUI.color = temp;
                yield return null;
            }

            cur = 0;
            per = 0;
            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.3f);

                temp.a = Mathf.Lerp(150 / 225f, 0, per);
                playerHitUI.color = temp;
                yield return null;
            }
        }
    }
}
