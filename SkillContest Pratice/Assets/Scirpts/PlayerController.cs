using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 회전 값
    readonly private float MAX_ROTATEX = 30f;
    readonly private float MIN_ROTATEX = -30f;
    readonly private float MAX_ROTATEZ = 10f;
    readonly private float MIN_ROTATEZ = -30f;
    #endregion
    #region 딜레이 값
    [SerializeField] private float playerShotDelayTime;
    #endregion

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float returnRotateSpeed;
    [SerializeField] private float boosterSpeed;

    [Space(10)]
    [SerializeField] private GameObject model;
    [SerializeField] private PlayerBullet bullet;
    [SerializeField] private Transform[] firePos;

    [SerializeField] private TrailRenderer[] boosterEffect;
    [SerializeField] private GameObject[] airEffect;

    private Vector3 moveInput;
    private float rotateX = 0;
    private float rotateZ = 0;

    private float curTime = 0;
    private float tempTime = 0;
    private int shotPointIndex = 0;

    private bool isbooster = false;
    private bool isBooster 
    {
        get { return isbooster; }
        set
        {
            if(isbooster != value)
            {
                isbooster = value;
                StartCoroutine(boosterEffectOnOff(isbooster, 1f));
            }           
        } 
    }

    void FixedUpdate()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var moveDir = transform.forward + moveInput;

        PlayerRotation(moveInput);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    private void Update()
    {
        PlayerShot();
        PlayerBooster();
    }

    private void PlayerRotation(Vector3 moveInput)
    {
        if (moveInput != Vector3.zero)
        {
            rotateX = Mathf.Clamp(rotateX + moveInput.x * rotateSpeed * Time.deltaTime, MIN_ROTATEX, MAX_ROTATEX);
            rotateZ = Mathf.Clamp(rotateZ + moveInput.y * rotateSpeed * Time.deltaTime, MIN_ROTATEZ, MAX_ROTATEZ);
        }
        else
        {
            rotateX = Mathf.Lerp(rotateX, 0, returnRotateSpeed * Time.deltaTime);
            rotateZ = Mathf.Lerp(rotateZ, 0, returnRotateSpeed * Time.deltaTime);
        }

        model.transform.eulerAngles = new Vector3(rotateX, model.transform.eulerAngles.y, -rotateZ);
    }
    private void PlayerShot()
    {
        curTime += Time.deltaTime;
        if (curTime >= tempTime && Input.GetKey(KeyCode.Z))
        {
            tempTime = curTime + playerShotDelayTime;
            shotPointIndex = (shotPointIndex == 0) ? 1 : 0;

            Instantiate(bullet, firePos[shotPointIndex].position, bullet.transform.rotation);
        }
    }
    private void PlayerBooster()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBooster = !isBooster;
        }
    }
    private IEnumerator boosterEffectOnOff(bool isBooster, float time)
    {
        var value = (isBooster) ? 1 : 0;
        var speedMulValue = (isBooster) ? 1 : -1;

        for (int i = 0; i < airEffect.Length; i++) airEffect [i].SetActive(isBooster);
        moveSpeed = speedMulValue * boosterSpeed + moveSpeed;

        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            for (int i = 0; i < boosterEffect.Length; i++)
            {
                boosterEffect[i].time = Mathf.Lerp(boosterEffect[i].time, value, percent);
            }
            yield return null;
        }
        yield break;
    }
}
