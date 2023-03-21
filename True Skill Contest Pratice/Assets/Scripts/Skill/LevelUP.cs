using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelUP : MonoBehaviour
{
    public Dictionary<PassiveType, int> passiveLevels;

    private void Awake()
    {
        
    }

}

public enum PassiveType
{
    HpUP,
    FuelUP,
    Invis,
    BulletUP,
    CircleBullet,
    Rayzer,
    TurnBullet,
}
