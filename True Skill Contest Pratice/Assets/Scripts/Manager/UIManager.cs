using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image playerHitUI;
    [SerializeField] private Image targetSightUI;
    [SerializeField] private TextMeshProUGUI qusetUI;

    private void Awake() => SetInstance();

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {

    }

    public void QusetUIUpdate(string text)
    {
        qusetUI.text = text;
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
}
