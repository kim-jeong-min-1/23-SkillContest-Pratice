using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRayzer : MonoBehaviour
{
    public float rayzerDamage { get; set; }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")) other.GetComponent<Enemy>().GetDamage(rayzerDamage);
        else if (other.CompareTag("Boss")) other.GetComponent<Boss>().GetDamage(rayzerDamage);
    }
}
