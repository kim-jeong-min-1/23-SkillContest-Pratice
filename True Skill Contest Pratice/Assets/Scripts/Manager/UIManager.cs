using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image targetSightUI;
    [SerializeField] private TextMeshProUGUI qusetUI;

    [SerializeField] private Image playerSkill_1UI;
    [SerializeField] private Image playerSkill_2UI;
    [SerializeField] private Image flashUI;
    [SerializeField] private Text skillDisableText;

    private void Awake() => SetInstance();

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {

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
            Color tempColor = new Color(1, 1, 1, 1);
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

            yield break;
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
}
