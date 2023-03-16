using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils>
{
    public static readonly Vector2 limit = new Vector2(55f, 30f);

    private void Awake()
    {
        SetInstance();
    }
}
