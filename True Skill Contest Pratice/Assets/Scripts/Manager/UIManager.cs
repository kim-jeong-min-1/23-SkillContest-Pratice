using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject interfaceUIGroup;
    [Space(20f)]
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image targetSightUI;
    [SerializeField] private TextMeshProUGUI qusetUI;
    [SerializeField] private TextMeshProUGUI scoreUI;

    [Space(20f)]
    [SerializeField] private Image playerSkill_1UI;
    [SerializeField] private Image playerSkill_2UI;
    [SerializeField] private Image flashUI;
    [SerializeField] private Text skillDisableText;

    [Space(20f)]
    [SerializeField] private GameObject skillSelectUIGroup;
    [SerializeField] private List<SelectUI> selectUIs;

    [SerializeField] public InputField inputField;
    [SerializeField] private RectTransform stageUI;
    [SerializeField] private StageUI StageUI;

    private void Awake() => SetInstance();

    public void SetStageUI(string text, int score, System.Action action)
    {
        StageUI.mainText.text = text;
        StageUI.scoreText.text = $"Score : {score}";

        var time = Time.unscaledTime;
        var m = (int)time / 60;
        var s = (int)time % 60;
        StageUI.timeText.text = $"Time : {m}m {s}s";

        StageUI.nextbtn.onClick.RemoveAllListeners();
        StageUI.nextbtn.onClick.AddListener(() => action());
    }
    public void StageUIOn(float time)
    {
        StartCoroutine(StageClearUIOn(time));
        IEnumerator StageClearUIOn(float time)
        {
            float current = 0;
            float percent = 0;
            var start = new Vector3(0, -1000f, 0);

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / time;

                stageUI.anchoredPosition = Vector3.Lerp(start, new Vector3(0, 20, 0), percent);
                yield return null;
            }
        }
    }
    public void StageUIOff(float time)
    {
        StartCoroutine(StageClearUIOn(time));
        IEnumerator StageClearUIOn(float time)
        {
            float current = 0;
            float percent = 0;
            var start = new Vector3(0, 20f, 0);

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / time;

                stageUI.anchoredPosition = Vector3.Lerp(start, new Vector3(0, -1000, 0), percent);
                yield return null;
            }
        }
    }
    public void OnInputField()
    {
        inputField.gameObject.SetActive(true);

        StageUI.timeText.gameObject.SetActive(false);
        StageUI.scoreText.gameObject.SetActive(false);
        StageUI.nextbtn.gameObject.SetActive(false);
    }

    public void SetSelectUI(int index, Skill skill)
    {
        var select = selectUIs[index];

        select.nameText.text = $"{skill.name}";
        select.explainText.text = $"{skill.explain}";
        select.levelText.text = $"Level : {skill.level}";
    }
    public void SelectUIUpdate(int index)
    {
        StartCoroutine(SelectUIUpdate());
        IEnumerator SelectUIUpdate()
        {
            for (int i = 0; i < selectUIs.Count; i++)
            {
                Vector3 startPos = new Vector3(selectUIs[i].offsetX, 20f);
                Vector3 endPos;

                if (i == index) endPos = new Vector3(selectUIs[i].offsetX, 100f);
                else endPos = new Vector3(selectUIs[i].offsetX, 20f);

                if (i == selectUIs.Count - 1)
                    yield return StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.25f * Time.timeScale));
                else
                    StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.25f * Time.timeScale));

                yield return null;
            }
        }

    }
    public void SkillSelectOn()
    {
        StartCoroutine(SkillSelectUIOn());

        IEnumerator SkillSelectUIOn()
        {
            skillSelectUIGroup.SetActive(true);
            for (int i = 0; i < selectUIs.Count; i++)
            {
                var startPos = new Vector3(selectUIs[i].offsetX, -1000f);
                var endPos = new Vector3(selectUIs[i].offsetX, 20f);

                if (i == selectUIs.Count - 1)
                    yield return StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));
                else
                    StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));

                yield return new WaitForSeconds(0.02f);
            }
        }
    }
    public void SkillSelectUIOff()
    {
        StartCoroutine(SkillSelectUIOff());

        IEnumerator SkillSelectUIOff()
        {
            for (int i = 0; i < selectUIs.Count; i++)
            {
                var startPos = new Vector3(selectUIs[i].offsetX, 20f);
                var endPos = new Vector3(selectUIs[i].offsetX, -1000f);

                if (i == selectUIs.Count - 1)
                    yield return StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));
                else
                    StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));
            }
            skillSelectUIGroup.SetActive(false);
        }
    }

    public void Skill1_UIUpdate(float value) => playerSkill_1UI.fillAmount = value;
    public void Skill2_UIUpdate(float value) => playerSkill_2UI.fillAmount = value;
    public void SkillDisable()
    {
        StartCoroutine(skillDisable());

        IEnumerator skillDisable()
        {
            skillDisableText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            skillDisableText.gameObject.SetActive(false);
        }
    }

    public void QusetUIUpdate(string text) => qusetUI.text = text;
    public void ScoreUIUpdate(string text) => scoreUI.text = text;
    public void TargetSightUpdate(Vector3 pos)
    {
        var targetPos = Camera.main.WorldToScreenPoint(pos);
        targetSightUI.rectTransform.position = targetPos;
    }

    public void PlayerHitUIEffect(float time)
    {
        StartCoroutine(PlayerHitUIEffect(time));
        IEnumerator PlayerHitUIEffect(float time)
        {
            Color tempColor = Color.white;
            float current = 0;
            float percent = 0;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.5f);

                tempColor.a = Mathf.Lerp(tempColor.a, 180f / 255, percent);
                playerHitUI.color = tempColor;
                yield return null;
            }

            current = 0;
            percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.5f);

                tempColor.a = Mathf.Lerp(tempColor.a, 0f, percent);
                playerHitUI.color = tempColor;

                yield return null;
            }
        }
    }
    public void FlashEffect(float time)
    {
        StartCoroutine(FlashEffect(time));

        IEnumerator FlashEffect(float time)
        {
            float current = 0;
            float percent = 0;
            Color tempColor = flashUI.color;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.3f);

                tempColor.a = Mathf.Lerp(0, 1, percent);
                flashUI.color = tempColor;

                yield return null;
            }

            current = 0; percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.7f);

                tempColor.a = Mathf.Lerp(1, 0, percent);
                flashUI.color = tempColor;
                yield return null;
            }
        }
    }

    public void InterfaceEnable(bool enable) => interfaceUIGroup.SetActive(enable);
    private IEnumerator UIMovement(Image UI, Vector3 start, Vector3 end, float time)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            UI.rectTransform.anchoredPosition = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
}

[System.Serializable]
public struct SelectUI
{
    public Image ui;
    public float offsetX;

    public Text nameText;
    public Text levelText;
    public Text explainText;
}

[System.Serializable]
public struct StageUI
{
    public Text mainText;
    public Text scoreText;
    public Text timeText;
    public Button nextbtn;
}
