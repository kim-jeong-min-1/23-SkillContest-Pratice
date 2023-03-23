using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameDirector : Singleton<GameDirector>
{
    private void Awake()
    {
        SetInstance();
    }



    public static IEnumerator ObjectMove(Transform obj, Vector3 end, float time)
    {
        float percent = 0;
        float current = 0;
        Vector3 start = obj.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            obj.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
    public static IEnumerator CameraZoom(Camera camera, float size, float time)
    {
        float percent = 0;
        float current = 0;
        float start = camera.fieldOfView;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            camera.fieldOfView = Mathf.Lerp(start, size, percent);
            yield return null;
        }
    }
    public static IEnumerator CameraMove(Camera camera, Vector3 movePos, float time)
    {
        float percent = 0;
        float current = 0;
        Vector3 start = camera.transform.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            camera.transform.position = Vector3.Lerp(start, movePos, percent);
            yield return null;
        }
    }
}
