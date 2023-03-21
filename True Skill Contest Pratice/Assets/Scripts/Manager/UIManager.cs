using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Space(20f)]
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image targetSightUI;
    [SerializeField] private TextMeshProUGUI qusetUI;

    [Space(20f)]
    [SerializeField] private Image playerSkill_1UI;
    [SerializeField] private Image playerSkill_2UI;
    [SerializeField] private Image flashUI;
    [SerializeField] private Text skillDisableText;

    [Space(20f)]
    [SerializeField] private GameObject skillSelectUIGroup;
    [SerializeField] private List<SelectUI> selectUIs;

    private void Awake() => SetInstance();

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {

    }

    public void SkillSelectUpUIOn()
    {
        StartCoroutine(SkillSelectUpUIOn());

        IEnumerator SkillSelectUpUIOn()
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
            SkillSelectUpUIOff();
        }
    }
    public void SkillSelectUpUIOff()
    {
        StartCoroutine(SkillSelectUpUIOff());

        IEnumerator SkillSelectUpUIOff()
        {
            for (int i = 0; i < selectUIs.Count; i++)
            {
                var startPos = new Vector3(selectUIs[i].offsetX, 20f);
                var endPos = new Vector3(selectUIs[i].offsetX, -1000f);

                if (i == selectUIs.Count - 1)
                    yield return StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));
                else
                    StartCoroutine(UIMovement(selectUIs[i].ui, startPos, endPos, 0.6f * Time.timeScale));

                yield return new WaitForSeconds(0.02f);
            }
            skillSelectUIGroup.SetActive(false);
        }
    }

    public void Skill1_UIUpdate(float value) => playerSkill_1UI.fillAmount = value;
    public void Skill2_UIUpdate(float value) => playerSkill_2UI.fillAmount = value;
    public void QusetUIUpdate(string text) => qusetUI.text = text;
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
