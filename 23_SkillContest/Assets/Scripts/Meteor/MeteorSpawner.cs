using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private Transform meteorGroup;
    [SerializeField] private Meteor meteor;

    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (!GameManager.Instance.isBoss)
        {
            var randPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);

            Meteor meteor = Instantiate(this.meteor, randPos, Quaternion.identity, meteorGroup);
            MeteorSubject.Instance.AddEnemy(meteor);

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
