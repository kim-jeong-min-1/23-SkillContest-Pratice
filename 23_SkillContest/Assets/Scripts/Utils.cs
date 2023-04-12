using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 moveLimit = new Vector2(50f, 20f);
    public static Vector2 spawnLimit = new Vector2(15f, 35f);

    public static bool OutCheck(Vector3 Pos)
    {
        if (Pos.x >= 60f || Pos.x <= -60f ||
            Pos.z >= 50f || Pos.z <= -24f)
        {
            return true;
        }
        return false;
    }
}
