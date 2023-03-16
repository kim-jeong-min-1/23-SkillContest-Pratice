using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils>
{
    public static readonly Vector2 limit = new Vector2(65f, 15f);

    private void Awake()
    {
        SetInstance();
    }
}
