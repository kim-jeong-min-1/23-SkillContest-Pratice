using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Camera directorCamera;
    private Vector3 directCamOffset = new Vector3(0, 100, -60);

    [SerializeField] private Text directText;
    [SerializeField] private Image fade;
    [SerializeField] private GameObject latterBox;
    [SerializeField] private GameObject explosion;

    public void StageStart(Transform playerObj, string stageName)
    {
        StartCoroutine(StageStart(playerObj, stageName));
        IEnumerator StageStart(Transform playerObj, string stageName)
        {
            GameManager.Instance.GameStopOnOff();
            UIManager.Instance.InterfaceEnable(false);

            latterBox.SetActive(true);
            directorCamera.gameObject.SetActive(true);
            directorCamera.transform.position = Camera.main.transform.position + directCamOffset;

            StartCoroutine(CameraZoom(20f, 0.1f));
            yield return StartCoroutine(FadeOut(1f));

            yield return StartCoroutine(ObjectMove(playerObj, new Vector3(0, 0, -20), 2f));
            StartCoroutine(TextAppearance(stageName, 2.5f));

            directorCamera.transform.position = Camera.main.transform.position;
            yield return StartCoroutine(CameraZoom(65f, 2.5f));

            latterBox.SetActive(false);
            UIManager.Instance.InterfaceEnable(true);
            GameManager.Instance.GameStopOnOff();
            directorCamera.gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
        }
    }
    public void BossAppearance(Transform bossObj)
    {
        StartCoroutine(BossAppearance(bossObj));
        IEnumerator BossAppearance(Transform bossObj)
        {
            GameManager.Instance.GameStopOnOff();
            UIManager.Instance.InterfaceEnable(false);
            directorCamera.gameObject.SetActive(true);

            yield return StartCoroutine(FadeIn(1.5f));
            StartCoroutine(FadeOut(1.5f));

            latterBox.SetActive(true);
            directorCamera.transform.position = (bossObj.position + directCamOffset) - Vector3.forward * 35;
            yield return StartCoroutine(CameraZoom(38f, 0.8f));

            var pos = bossObj.position - Vector3.forward * 35;
            StartCoroutine(ObjectMove(bossObj, pos, 2f));
            yield return new WaitForSecondsRealtime(1);
            yield return StartCoroutine(FadeIn(0.8f));

            latterBox.SetActive(false);
            directorCamera.gameObject.SetActive(false);
            UIManager.Instance.InterfaceEnable(true);
            GameManager.Instance.GameStopOnOff();
        }
    }
    public void BossDie(Transform bossObj)
    {
        StartCoroutine (BossDie(bossObj));
        IEnumerator BossDie(Transform bossObj)
        {
            yield return StartCoroutine(FadeOut(0.01f));
            GameManager.Instance.GameStopOnOff();
            UIManager.Instance.InterfaceEnable(false);
            directorCamera.gameObject.SetActive(true);
            latterBox.SetActive(true);

            var movepos = (bossObj.position + directCamOffset);
            StartCoroutine(CameraMove(movepos, 2.5f));
            yield return StartCoroutine(CameraZoom(45f, 2.5f));

            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                var nx = Random.Range(-12, 12); var nz = Random.Range(-12, 12);
                var pos = bossObj.position + new Vector3(nx, 0, nz);
                Instantiate(explosion, pos, Quaternion.identity);
            }
            Destroy(bossObj.gameObject);

            GameManager.Instance.GameStopOnOff();
            UIManager.Instance.InterfaceEnable(true);
            directorCamera.gameObject.SetActive(false);
            latterBox.SetActive(true);
        }
    }
    public void PlayerDie(Transform playerObj)
    {
        StartCoroutine(PlayerDie());
        IEnumerator PlayerDie()
        {
            GameManager.Instance.GameStopOnOff();
            yield return StartCoroutine(CameraZoom(60f, 0.01f));
            yield return StartCoroutine(FadeOut(0.01f));

            var movepos = playerObj.position + new Vector3(0, 50f, -18f);
            StartCoroutine(CameraMove(movepos, 2.5f));

            directorCamera.gameObject.SetActive(true);
            yield return StartCoroutine(CameraZoom(30f, 2.5f));

            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSecondsRealtime(0.08f);
                var nx = Random.Range(-1.5f, 1.5f); var nz = Random.Range(-1.5f, 1.5f);
                var pos = playerObj.position + new Vector3(nx, 0, nz);
                Instantiate(explosion, pos, Quaternion.identity);
            }
            GameManager.Instance.GameStopOnOff(true);
        }
    }

    public IEnumerator ObjectMove(Transform obj, Vector3 end, float time)
    {
        float percent = 0;
        float current = 0;
        Vector3 start = obj.position;

        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / time;

            obj.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
    public IEnumerator CameraZoom(float size, float time)
    {
        float percent = 0;
        float current = 0;
        float start = directorCamera.fieldOfView;

        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / time;

            directorCamera.fieldOfView = Mathf.Lerp(start, size, percent);
            yield return null;
        }
    }
    public IEnumerator CameraMove(Vector3 movePos, float time)
    {
        float percent = 0;
        float current = 0;
        Vector3 start = directorCamera.transform.position;

        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / time;

            directorCamera.transform.position = Vector3.Lerp(start, movePos, percent);
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        float curret = 0;
        Color temp = fade.color;
        temp.a = 0;

        while (curret / time < 1)
        {
            curret += Time.unscaledDeltaTime;

            temp.a = Mathf.Lerp(0f, 1f, curret / time);
            fade.color = temp;
            yield return null;
        }
    }
    public IEnumerator FadeOut(float time)
    {
        float curret = 0;
        Color temp = fade.color;
        temp.a = 1;

        while (curret / time < 1)
        {
            curret += Time.unscaledDeltaTime;

            temp.a = Mathf.Lerp(1f, 0f, curret / time);
            fade.color = temp;
            yield return null;
        }
    }
    public IEnumerator TextAppearance(string text, float time)
    {
        directText.text = $"[  {text}  ]";
        float current = 0;
        float percent = 0;
        Color tempColor = directText.color;

        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / (time * 0.5f);

            tempColor.a = Mathf.Lerp(0, 1, percent);
            directText.color = tempColor;

            yield return null;
        }

        current = 0; percent = 0;
        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / (time * 0.5f);

            tempColor.a = Mathf.Lerp(1, 0, percent);
            directText.color = tempColor;
            yield return null;
        }
    }
}
