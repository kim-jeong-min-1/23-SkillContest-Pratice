using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    public static Skill CreateSkill(SkillType type)
    {
        Skill n;

        switch (type)
        {
            case SkillType.HpUP:
                n = new HpUP();
                break;
            case SkillType.Rayzer:
                n = new Rayzer();
                break;
            case SkillType.BulletUP:
                n = new BulletUP();
                break;
            case SkillType.Invincible:
                n = new Invincible();
                break;
            case SkillType.Circle:
                n = new Circle();
                break;
            case SkillType.OneLap:
                n = new OneLap();
                break;
            default:
                n = new FuelUP();
                break;
        }

        return n;
    }
}
