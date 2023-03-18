using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils>
{
    public static readonly Vector2 moveLimit = new Vector2(55f, 30f);
    public static readonly Vector2 spawnLimit = new Vector2(55f, 80f);

    public static bool ObjectOutCheck(Vector3 pos)
    {
        if(Mathf.Abs(pos.x) >= 105f || (pos.z <= -52f || pos.z >= 120f))
        {
            return true;
        }
        return false;   
    }

    private void Awake()
    {
        SetInstance();
    }
}
