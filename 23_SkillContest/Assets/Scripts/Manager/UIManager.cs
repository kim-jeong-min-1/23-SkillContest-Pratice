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
    [SerializeField] private Text questText;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject skillBlockUI;
    [SerializeField] private List<SelectUI> selectUIs;
    [SerializeField] private GameObject selectUIGroup;
    [SerializeField] private StageUI stageUI;
    [SerializeField] private Image targetSight;
    public InputField inputField;
    public GameObject IngameUIGroup;


    void Update()
    {
        questText.text = $"Enemy {EnemySubject.Instance.enemyCount} / {GameManager.Instance.Quest}";
        scoreText.text = $"Score : {GameManager.Instance.Score}";
    }

    public void SetSkill_1UI(float time) => skill_1UI.fillAmount = time;
    public void SetSkill_2UI(float time) => skill_2UI.fillAmount = time;
    public void SkillBlock(float time)
    {
        StartCoroutine(skillBlock());
        IEnumerator skillBlock()
        {
            skillBlockUI.SetActive(true);
            yield return new WaitForSeconds(time);
            skillBlockUI.SetActive(false);
        }
    }

    public void FlashEffect(float time)
    {
        StartCoroutine(flash());
        IEnumerator flash()
        {
            float cur = 0;
            float per = 0;
            Color tempColor = flashUI.color;

            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.3f);

                tempColor.a = Mathf.Lerp(0, 1, per);
                flashUI.color = tempColor;
                yield return null;
            }
            cur = 0;
            per = 0;
            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / (time * 0.7f);

                tempColor.a = Mathf.Lerp(1, 0, per);
                flashUI.color = tempColor;
                yield return null;
            }
        }
    }
    public void PlayerHitEffect(float time)
    {
        StartCoroutine(playerHitEffect());
        IEnumerator playerHitEffect()
        {
            float cur = 0;
            float per = 0;
            Color tempColor = playerHitUI.color;

            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / time;

                tempColor.a = Mathf.Lerp(0f, 220 / 255f, per);
                playerHitUI.color = tempColor;
                yield return null;
            }
            cur = 0;
            per = 0;
            while (per < 1)
            {
                cur += Time.deltaTime;
                per = cur / time;

                tempColor.a = Mathf.Lerp(220 / 255f, 0f, per);
                playerHitUI.color = tempColor;
                yield return null;
            }
        }
    }

    public void SelectUIActive(bool acitve) => selectUIGroup.SetActive(acitve);
    public void SetSelectUI(Skill[] skills)
    {
        for (int i = 0; i < selectUIs.Count; i++)
        {
            selectUIs[i].nameText.text = skills[i].name;
            selectUIs[i].levelText.text = $"Level : {((skills[i].level == 4) ? "Max" : skills[i].level)}";
            selectUIs[i].explainText.text = skills[i].explain;
        }
    }
    public void ChangeSelect(int index)
    {
        for (int i = 0; i < selectUIs.Count; i++)
        {
            if(i == index)
            {
                StartCoroutine(UIMove(selectUIs[i].rect, new Vector2(selectUIs[i].rect.anchoredPosition.x, 50f), 0.1f));
            }
            else
            {
                StartCoroutine(UIMove(selectUIs[i].rect, new Vector2(selectUIs[i].rect.anchoredPosition.x, 0f), 0.1f));
            }
        }
    }
    public IEnumerator SelectUIMove(float y)
    {
        for (int i = 0; i < selectUIs.Count; i++)
        {
            if (i == selectUIs.Count - 1)
            {
                yield return StartCoroutine(UIMove(selectUIs[i].rect, new Vector2(selectUIs[i].rect.anchoredPosition.x, y), 0.8f));
            }
            else StartCoroutine(UIMove(selectUIs[i].rect, new Vector2(selectUIs[i].rect.anchoredPosition.x, y), 0.8f));
        }
    }

    public void StageUIOn(string main, int score, System.Action next)
    {
        stageUI.mainText.text = main;
        stageUI.scoreText.text = $"Score : {score}";

        var curTime = Time.time;
        var m = (int)curTime / 60;
        var s = (int)curTime % 60;

        stageUI.timeText.text = $"Time : {m}m {s}s";

        stageUI.nextBtn.onClick.RemoveAllListeners();
        stageUI.nextBtn.onClick.AddListener(() => next());
        stageUI.nextBtn.onClick.AddListener(() => SoundManager.Instance.PlaySfx(SoundEffect.Button2, 1f));
        StartCoroutine(UIMove(stageUI.rect, new Vector2(0, 0), 1f));
    }
    public void StageUIOff()
    {
        StartCoroutine(UIMove(stageUI.rect, new Vector2(0, -1000f), 1f));
    }

    public void InputFieldOn() => inputField.gameObject.SetActive(true);
    public void InGameUIOnOff(bool active) => IngameUIGroup.SetActive(active);

    public void TargetSightUpdate(Vector3 pos) => targetSight.transform.position = pos;
    private IEnumerator UIMove(RectTransform ui, Vector2 end, float time)
    {
        float cur = 0;
        float per = 0;
        Vector2 start = ui.anchoredPosition;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            ui.anchoredPosition = Vector2.Lerp(start, end, per);
            yield return null;
        }
    }
}

[System.Serializable]
public class SelectUI
{
    public RectTransform rect;
    public Text nameText;
    public Text levelText;
    public Text explainText;
}

[System.Serializable]
public class StageUI
{
    public RectTransform rect;
    public Text mainText;
    public Text scoreText;
    public Text timeText;
    public Button nextBtn;
}
