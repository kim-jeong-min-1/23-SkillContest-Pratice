using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 moveLimit = new Vector2(35f, 20f);

    public static bool OutCheck(Vector3 pos)
    {
        if((pos.x >= 60 || pos.x <= -60) 
            || (pos.z >= 50 || pos.z <= -25))
        {
            return true;
        }
        return false;
    }
}
