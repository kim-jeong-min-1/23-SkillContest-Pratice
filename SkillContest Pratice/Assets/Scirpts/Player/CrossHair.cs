using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CrossHair : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [SerializeField] private Transform targetSight;

    public Vector3 crossHairDir
    {
        get
        {
            return Camera.main.ScreenPointToRay(transform.position).direction;
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

        while (target != null)
        {            
            var sightPos = Camera.main.WorldToScreenPoint(target.position);
            targetSight.position = sightPos;

            yield return new WaitForEndOfFrame();
        }

        targetSight.gameObject.SetActive(false);
        yield break;
    }

    private void findTarget()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(transform.position);
        Physics.Raycast(Camera.main.ScreenToWorldPoint(transform.position), crossHairDir, out hit);

        if (hit.collider != null)
        {
            var target = hit.collider.GetComponent<Enemy>();
            if (target != null) this.target = target.transform;
        }
    }
}
