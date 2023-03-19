using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int curStagetNum { get; private set; } = 0;
    public int qusetCondtion { get; private set; }
    public bool qusetComplete { get; set; }

    [SerializeField] private List<Boss> bossPrefabs;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }

    private void SetGameManager()
    {
        curStagetNum++;
        qusetComplete = false;
        EnemySubject.Instance.enemyCount = 0;

        SetQuset();
    }

    private void SetQuset()
    {
        int conditon = curStagetNum switch
        {
            1 => 100,
            2 => 150,
            3 => 200,
            _ => 0
        };
        qusetCondtion = conditon;
    }

    private void Update()
    {
        UIManager.Instance.QusetUIUpdate($"Enemy {EnemySubject.Instance.enemyCount} / {qusetCondtion}");

        if (EnemySubject.Instance.enemyCount == qusetCondtion) qusetComplete = true;
    } 
}

