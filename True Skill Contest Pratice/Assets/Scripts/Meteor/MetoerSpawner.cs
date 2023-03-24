using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetoerSpawner : MonoBehaviour
{
    [SerializeField] private Meteor metoer;
    [SerializeField] private Transform meteorGroup;
    [SerializeField] private float maxDelay;
    [SerializeField] private float minDelay;

    void Start()
    {
        StartCoroutine(Enemy_Spawner());
    }

    private IEnumerator Enemy_Spawner()
    {
        while (!GameManager.Instance.qusetComplete)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            Vector3 spawnPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);
            Meteor meteor = Instantiate(metoer, spawnPos, Quaternion.identity, meteorGroup);
            MeteorSubject.Instance.AddMeteor(meteor);

        }
    }
}
