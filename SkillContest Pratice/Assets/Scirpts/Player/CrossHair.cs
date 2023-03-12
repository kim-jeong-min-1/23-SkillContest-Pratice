using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private Transform targetSight;
    public Transform target;

    public Vector3 crossHairDir
    {
        get
        {
            return Camera.main.ScreenPointToRay(transform.position).direction;
        }
    }
    public Vector3 crossHairPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(transform.position);
        }
    }

    private void Awake() => StartCoroutine(CrossHairUpdate());
    private IEnumerator CrossHairUpdate()
    {
        while (true)
        {
            findTarget();

            if (target != null)
            {
                yield return StartCoroutine(aimmingTarget());
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator aimmingTarget()
    {
        targetSight.gameObject.SetActive(true);
        target.gameObject.layer = LayerMask.NameToLayer("Target");

        while (target != null)
        {
            var sightPos = Camera.main.WorldToScreenPoint(target.position);
            targetSight.position = sightPos;

            if (!targetRangeCheck())
            {
                target.gameObject.layer = LayerMask.NameToLayer("Enemy");
                target = null;
            }

            yield return new WaitForEndOfFrame();
        }

        targetSight.gameObject.SetActive(false);
        yield break;
    }
    private bool targetRangeCheck()
    {
        var cameraheight = Camera.main.orthographicSize * 2;
        var cameraWidth = cameraheight * Camera.main.aspect;

        var cameraSize = new Vector3(cameraWidth, cameraheight);
        var cameraMain = Camera.main.transform;

        return Physics.BoxCast(crossHairPosition,
            cameraSize, cameraMain.forward, cameraMain.rotation, Mathf.Infinity, LayerMask.GetMask("Target"));     
    }
    private void findTarget()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(transform.position);
        Physics.Raycast(crossHairPosition, crossHairDir, out hit);

        if (hit.collider != null)
        {
            var target = hit.collider.GetComponent<Enemy>();
            if (target != null) this.target = target.transform;
        }
    }
}
