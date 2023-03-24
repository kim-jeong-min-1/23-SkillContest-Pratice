using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSubject : DestroySingleton<MeteorSubject>
{
    [SerializeField] private ParticleSystem destroyEffect;
    private List<Meteor> meteors;
    private List<Meteor> destroyMeteors;

    private void Awake()
    {
        SetInstance();
        SetVariable();
    }

    private void SetVariable()
    {
        meteors = new List<Meteor>();
        destroyMeteors = new List<Meteor>();
    }

    private void Update()
    {
        MeteorUpdate();
        MeteorDestroy();
    }

    private void MeteorUpdate()
    {
        foreach (var meteor in meteors)
        {
            if (meteor == null) continue;
            meteor.MeteorUpdate();
            if (meteor.isDestroy || Utils.ObjectOutCheck(meteor.transform.position))
            {
                destroyMeteors.Add(meteor);
            }
        }
    }

    private void MeteorDestroy()
    {
        foreach (var meteor in destroyMeteors)
        {
            if (meteor.isDestroy) GameManager.Instance.AddScore(meteor.score);

            Destroy(meteor.gameObject);
            DestroyEffect(meteor.transform.position);
            meteors.Remove(meteor);
        }
        destroyMeteors.Clear();
    }

    private void DestroyEffect(Vector3 pos)
    {
        Instantiate(destroyEffect, pos, Quaternion.identity);
    }

    public void AddMeteor(Meteor meteor)
    {
        meteors.Add(meteor);
    }
}
