using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    HpUP,
    FuelUP,
    Invicible,
    Rayzer,
    Circle,

}

public abstract class Skill : MonoBehaviour
{
    public int level { get; set; }

    public float coolTime;
    public float curTime;
    public string skillName;
    public string explain;

    public abstract void UseSkill();
}


