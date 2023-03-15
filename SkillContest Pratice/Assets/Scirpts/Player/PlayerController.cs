using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.Rendering;
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

    [Space(15)]
    [SerializeField] private GameObject model;
    [SerializeField] private Transform[] firePos;
    [SerializeField] private PlayerBullet playerBullet;
    [SerializeField] private PlayerMissile playerMissile;
    [SerializeField] private CrossHair crossHair;

    [Space(15)]
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
                StartCoroutine(boosterOnOff(isbooster, 1f));
            }           
        } 
    }

    void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation(moveInput);
    }
    private void Update()
    {
        PlayerShot();
        PlayerMissile();
        PlayerBooster();
    }

    private void PlayerMovement()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var moveDir = transform.forward + moveInput;
        
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
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

            Quaternion dir;

            if(crossHair.target != null)
            {
                var vec = (crossHair.target.position - firePos[shotPointIndex].position).normalized;
                dir = Quaternion.LookRotation(vec, Vector3.one);
            }
            else
            {
                dir = Quaternion.Euler(crossHair.crossHairDir);
            }
            
            Instantiate(playerBullet, firePos[shotPointIndex].position, dir);
        }
    }
    private void PlayerMissile()
    {
        if (Input.GetKeyDown(KeyCode.A) && crossHair.target != null)
        {
            int pos = 6;
            for (int i = 0; i < 6; i++)
            {
                var missile = Instantiate
                    (playerMissile, 
                    new Vector3(pos + transform.position.x, transform.position.y, transform.position.z), 
                    Quaternion.Euler(-90, i * 30, i * 30));
                missile.target = crossHair.target;

                pos -= 2;
            }            
        }
    }
    private void PlayerBooster()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBooster = !isBooster;
        }
    }
    private IEnumerator boosterOnOff(bool isBooster, float time)
    {
        for (int i = 0; i < airEffect.Length; i++) airEffect [i].SetActive(isBooster);

        var speedMulValue = (isBooster) ? 1 : -1;
        moveSpeed = speedMulValue * boosterSpeed + moveSpeed;

        float current = 0;
        float percent = 0;
        var value = (isBooster) ? 1 : 0;

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
