using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private Transform meteorGroup;
    [SerializeField] private Meteor meteor;

    private void Start()
    {
        StartCoroutine(SpawnMeteor());
    }

    private IEnumerator SpawnMeteor()
    {
        while (true)
        {
            var randPos = new Vector3(Random.Range(-Utils.spawnLimit.x, Utils.spawnLimit.x), 0f, Utils.spawnLimit.y);

            Meteor meteor = Instantiate(this.meteor, randPos, Quaternion.identity, meteorGroup);
            MeteorSubject.Instance.AddMeteor(meteor);

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
