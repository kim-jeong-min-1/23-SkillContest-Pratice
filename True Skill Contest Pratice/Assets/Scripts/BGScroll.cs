using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    private readonly Vector3 restPosition = new Vector3(0, 0, 320);
    [SerializeField] private float scrollSpeed;

    void Update()
    {
        if(transform.position.z <= -250f) transform.position = restPosition;
        transform.Translate(-transform.forward * scrollSpeed * Time.deltaTime);      
    }
}
