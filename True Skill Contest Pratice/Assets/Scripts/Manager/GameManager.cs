using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int curStageNum { get; private set; } = 0;
    public int stageScore { get; private set; }
    public int qusetCondtion { get; private set; } = 0;

    public bool qusetComplete { get; set; } = false;
    public bool isCurStageClear { get; set; } = false;

    public Boss curStageBoss { get; set; } = null;
    public GameObject playerObject { get; set; } = null;

    private GameDirector gameDirector;
    [SerializeField] private List<Boss> bossPrefabs;
    [SerializeField] private List<string> stageNames;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }

    private void SetGameManager()
    {
        curStageNum++;

        qusetComplete = false;
        isCurStageClear = false;
        curStageBoss = null;

        playerObject = FindObjectOfType<PlayerController>().gameObject;
        gameDirector = FindObjectOfType<GameDirector>();
        SetQuset();

        gameDirector.StageStart(playerObject.transform, stageNames[curStageNum - 1]);
    }

    private void SetQuset()
    {
        int conditon = curStageNum switch
        {
            1 => 10,
            2 => 10,
            3 => 200,
            _ => 0
        };
        qusetCondtion = conditon;
    }
    public void AddScore(int score) => stageScore += score;

    private void NextStage()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(curStageNum + 1);
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            if (op.isDone)
            {
                SetGameManager();
                break;
            }
        }

        SetGameManager();
    }

    public void GameStopOnOff()
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

        if (curStageBoss == null)
        {
            if (qusetComplete)
            {
                curStageBoss
                = Instantiate(bossPrefabs[curStageNum - 1], new Vector3(0, 0, 100f), Quaternion.identity);
                gameDirector.BossAppearance(curStageBoss.transform);
            }
        }
        else
        {
            if (curStageBoss.isDie && !isCurStageClear)
            {
                // 스테이지 클리어 처리
                isCurStageClear = true;
                GameStopOnOff();
                UIManager.Instance.SetStageClearUI(stageScore, NextStage);
                UIManager.Instance.StageClearUIOn(1f);
            }
        }
    }
}

