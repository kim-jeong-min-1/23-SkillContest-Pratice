using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GameDirector : Singleton<GameDirector>
{
    [SerializeField] private Camera directorCamera;
    private Vector3 directCamOffset = new Vector3(0, 100, -60);

    [SerializeField] private Text directText;
    [SerializeField] private Image fade;

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
        while (curret / time < 1)
        {
            curret += Time.unscaledDeltaTime;
            yield break;
        }
    }
}
