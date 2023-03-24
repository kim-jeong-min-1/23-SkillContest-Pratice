using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int curStagetNum { get; private set; } = 0;
    public int stageScore { get; private set; }
    public int qusetCondtion { get; private set; } = 0;
    public bool qusetComplete { get; set; } = false;
    public Boss curStageBoss { get; set; } = null;
    public GameObject playerObject { get; set; } = null;


    [SerializeField] private List<Boss> bossPrefabs;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }

    private void SetGameManager()
    {
        EnemySubject.Instance.enemyCount = 0;
        curStagetNum++;
        qusetComplete = false;
        curStageBoss = null;
        playerObject = FindObjectOfType<PlayerController>().gameObject;
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
    public void AddScore(int score) => stageScore += score;
    public void DirectorSetChanage()
    {
        PlayerController.Instance.enabled = !PlayerController.Instance.enabled;
        PlayerSkillSystem.Instance.enabled = !PlayerSkillSystem.Instance.enabled;
        if (curStageBoss) curStageBoss.enabled = !curStageBoss.enabled;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }

    private void Update()
    {
        UIManager.Instance.QusetUIUpdate($"Enemy {EnemySubject.Instance.enemyCount} / {qusetCondtion}");
        UIManager.Instance.ScoreUIUpdate($"Score : {stageScore}");

        if (EnemySubject.Instance.enemyCount == qusetCondtion) qusetComplete = true;

        if (qusetComplete && curStageBoss == null)
        {
            curStageBoss
            = Instantiate(bossPrefabs[curStagetNum - 1], new Vector3(0, 0, 100f), Quaternion.identity);
        }

    }
}

