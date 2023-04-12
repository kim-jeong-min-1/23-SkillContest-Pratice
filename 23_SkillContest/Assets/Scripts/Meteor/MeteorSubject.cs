using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSubject : DestorySingleton<MeteorSubject>
{
    private List<Meteor> meteors;
    private List<Meteor> destroyMeteors;
    [SerializeField] private ParticleSystem destroyEffect;

    private void Awake()
    {
        SetInstance();
        meteors = new List<Meteor>();
        destroyMeteors = new List<Meteor>();
    }

    private void Update()
    {
        MeteorUpdate();
        DestroyMeteor();
    }

    private void MeteorUpdate()
    {
        foreach (var meteor in meteors)
        {
            if (meteor.isDestroy || Utils.OutCheck(meteor.transform.position))
            {
                destroyMeteors.Add(meteor);
            }
        }
    }

    private void DestroyMeteor()
    {
        foreach (var meteor in destroyMeteors)
        {
            if (meteor.isDestroy) GameManager.Instance.Score += meteor.score;

            meteors.Remove(meteor);
            Instantiate(destroyEffect, meteor.transform.position, Quaternion.identity);
            Destroy(meteor.gameObject);
        }
        destroyMeteors.Clear();
    }


    public void AddEnemy(Meteor meteor)
    {
        meteors.Add(meteor);
    }
}
