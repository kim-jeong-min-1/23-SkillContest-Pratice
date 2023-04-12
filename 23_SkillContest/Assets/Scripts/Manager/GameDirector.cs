using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Camera directorCam;
    [SerializeField] private Image fade;
    [SerializeField] private Text stageText;
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private GameObject Explosion;

    public void StageStart(string StageName)
    {
        StartCoroutine(StageStart());
        IEnumerator StageStart()
        {
            DierectorSet(false);
            UIManager.Instance.InGameUIOnOff(false);
            Time.timeScale = 0f;

            PlayerController.Instance.PlayerReset();
            StartCoroutine(CamerMove(new Vector3(0, 20, -25), 0.1f));
            yield return StartCoroutine(CamerZoom(40, 0.1f));

            StartCoroutine(FadeOut(1f));
            directorCam.gameObject.SetActive(true);

            yield return StartCoroutine(ObjectMove(PlayerController.Instance.transform, new Vector3(0, 0, -20f), 2f));

            StartCoroutine(TextAppearence(StageName, 4f));
            StartCoroutine(CamerMove(new Vector3(0, 20, -20), 2.5f));
            yield return StartCoroutine(CamerZoom(60, 3f));

            directorCam.gameObject.SetActive(false);
            DierectorSet(true);
            UIManager.Instance.InGameUIOnOff(true);
            Time.timeScale = 1f;
            yield return null;
        }
    }
    public void BossAppearence()
    {
        StartCoroutine(BossAppearence());
        IEnumerator BossAppearence()
        {
            DierectorSet(false);
            UIManager.Instance.InGameUIOnOff(false);
            Time.timeScale = 0f;

            StartCoroutine(CamerMove(Camera.main.transform.position, 0.1f));
            yield return StartCoroutine(CamerZoom(60, 0.1f));
            directorCam.gameObject.SetActive(true);
            yield return StartCoroutine(FadeIn(1.5f));

            StartCoroutine(CamerMove(new Vector3(0, 0, 35) + camOffset, 0.1f));
            StartCoroutine(CamerZoom(40, 0.1f));
            yield return StartCoroutine(FadeOut(1.5f));

            yield return StartCoroutine(ObjectMove(GameManager.Instance.curStageBoss.transform, new Vector3(0, 0, 38f), 2f));

            yield return StartCoroutine(FadeIn(1f));

            directorCam.gameObject.SetActive(false);
            DierectorSet(true);
            UIManager.Instance.InGameUIOnOff(true);
            Time.timeScale = 1f;

            yield return null;
        }
    }
    public void BossDie()
    {
        var boss = GameManager.Instance.curStageBoss.transform;
        StartCoroutine(BossAppearence());
        IEnumerator BossAppearence()
        {
            DierectorSet(false);
            UIManager.Instance.InGameUIOnOff(false);
            Time.timeScale = 0f;

            StartCoroutine(FadeOut(0.1f));
            StartCoroutine(CamerMove(boss.position + camOffset, 0.1f));
            yield return StartCoroutine(CamerZoom(60, 0.1f));
            directorCam.gameObject.SetActive(true);

            yield return StartCoroutine(CamerZoom(40, 2.5f));

            SoundManager.Instance.PlaySfx(SoundEffect.BossDie, 0.9f);
            for (int i = 0; i < 6; i++)
            {
                var randX = Random.Range(-6, 6);
                var randZ = Random.Range(-2, 2);
                Instantiate(Explosion, boss.position + new Vector3(randX, 0, randZ), Quaternion.identity);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(0.88f);
            Destroy(boss.gameObject);

            directorCam.gameObject.SetActive(false);
            DierectorSet(true);
            UIManager.Instance.InGameUIOnOff(true);
            Time.timeScale = 1f;
            yield return null;
        }
    }
    public void PlayerDie()
    {
        var player = PlayerController.Instance.transform;
        StartCoroutine(BossAppearence());
        IEnumerator BossAppearence()
        {
            DierectorSet(false);
            UIManager.Instance.InGameUIOnOff(false);
            Time.timeScale = 0f;

            StartCoroutine(FadeOut(0.1f));
            StartCoroutine(CamerMove(player.position + camOffset, 0.1f));
            yield return StartCoroutine(CamerZoom(60, 0.1f));
            directorCam.gameObject.SetActive(true);

            yield return StartCoroutine(CamerZoom(40, 2.5f));

            for (int i = 0; i < 6; i++)
            {
                var randX = Random.Range(-2, 2);
                var randZ = Random.Range(-1, 1);
                Instantiate(Explosion, player.position + new Vector3(randX, 0, randZ), Quaternion.identity);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(0.88f);

            directorCam.gameObject.SetActive(false);
            UIManager.Instance.InGameUIOnOff(true);
            Time.timeScale = 1f;   
            yield return null;
        }
    }

    private void DierectorSet(bool active)
    {
        PlayerController.Instance.enabled = active;
        PlayerController.Instance.playerSkill.enabled = active;
        if (GameManager.Instance.curStageBoss != null) GameManager.Instance.curStageBoss.enabled = active;
    }


    private IEnumerator ObjectMove(Transform obj, Vector3 end, float time)
    {
        float cur = 0;
        float per = 0;
        Vector3 start = obj.transform.position;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            obj.transform.position = Vector3.Lerp(start, end, per);
            yield return null;
        }
    }
    private IEnumerator CamerMove(Vector3 end, float time)
    {
        float cur = 0;
        float per = 0;
        Vector3 start = directorCam.transform.position;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            directorCam.transform.position = Vector3.Lerp(start, end, per);
            yield return null;
        }
    }
    private IEnumerator CamerZoom(float end, float time)
    {
        float cur = 0;
        float per = 0;
        float start = directorCam.fieldOfView;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            directorCam.fieldOfView = Mathf.Lerp(start, end, per);
            yield return null;
        }
    }
    private IEnumerator FadeIn(float time)
    {
        float cur = 0;
        float per = 0;
        Color fadeColor = fade.color;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            fadeColor.a = Mathf.Lerp(0, 1, per);
            fade.color = fadeColor;
            yield return null;
        }
    }
    private IEnumerator FadeOut(float time)
    {
        float cur = 0;
        float per = 0;
        Color fadeColor = fade.color;

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / time;

            fadeColor.a = Mathf.Lerp(1, 0, per);
            fade.color = fadeColor;
            yield return null;
        }
    }
    private IEnumerator TextAppearence(string text, float time)
    {
        float cur = 0;
        float per = 0;
        Color fadeColor = stageText.color;
        stageText.text = $"[  {text}  ]";

        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / (time * 0.3f);

            fadeColor.a = Mathf.Lerp(0, 1, per);
            stageText.color = fadeColor;
            yield return null;
        }
        cur = 0;
        per = 0;
        while (per < 1)
        {
            cur += Time.unscaledDeltaTime;
            per = cur / (time * 0.7f);

            fadeColor.a = Mathf.Lerp(1, 0, per);
            stageText.color = fadeColor;
            yield return null;
        }

    }
}
