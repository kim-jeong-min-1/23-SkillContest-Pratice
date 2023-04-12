using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int StageNum { get; private set; } = 0;
    public int Quest { get; private set; } = 0;
    public int Score { get; set; } = 0;
    public bool isBoss { get; private set; } = false;
    public Boss curStageBoss { get; private set; } = null;

    private bool isStageClear = false;

    [SerializeField] private List<Boss> stageBoss;
    [SerializeField] private List<string> stageName;
    [SerializeField] private GameDirector gameDirector;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }
    private void Start() => gameDirector.StageStart(stageName[StageNum - 1]);

    private void SetGameManager()
    {
        StageNum++;
        isBoss = false;
        isStageClear = false;
        curStageBoss = null;

        SetQuest(StageNum);
        SoundManager.Instance.PlayBgm(Bgm.Ingame, 0.8f);
    }

    private void SetQuest(int stageNum)
    {
        var quest = stageNum switch
        {
            1 => 50,
            2 => 80,
            3 => 100,
            _ => 0
        };
        Quest = quest;
    }

    private void Update()
    {
        QuestCheck();
        if (isBoss) ClearCheck();

        //치트키
        if (Input.GetKeyDown(KeyCode.Escape)) PlayerController.Instance.isDie = !PlayerController.Instance.isDie;
        else if (Input.GetKeyDown(KeyCode.F1)) EnemySubject.Instance.DestroyAllEnemy();
        else if (Input.GetKeyDown(KeyCode.F2)) PlayerController.Instance.ShooterLevel += 3;
        else if (Input.GetKeyDown(KeyCode.F3)) PlayerController.Instance.CoolTimeReset();
        else if (Input.GetKeyDown(KeyCode.F4)) PlayerController.Instance.HP = 200;
        else if (Input.GetKeyDown(KeyCode.F5)) PlayerController.Instance.Fuel = 1000;
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            if (curStageBoss) Destroy(curStageBoss.gameObject);
            NextStage();
        }
        else if (Input.GetKeyDown(KeyCode.F7)) EnemySubject.Instance.enemyCount = Quest;
        else if (Input.GetKeyDown(KeyCode.F8)) PlayerController.Instance.playerSkill.SelectSkill();

    }

    private void QuestCheck()
    {
        if (EnemySubject.Instance.enemyCount >= Quest && !isBoss)
        {
            isBoss = true;

            SoundManager.Instance.PlaySfx(SoundEffect.BossApear, 0.9f);
            curStageBoss = Instantiate(stageBoss[StageNum - 1], new Vector3(0, 0, 50), Quaternion.identity);
            curStageBoss.HpMult(StageNum);
            gameDirector.BossAppearence();
        }
    }

    private void ClearCheck()
    {
        if (curStageBoss.isDie && !isStageClear)
        {
            isStageClear = true;
            gameDirector.BossDie();

            if (PlayerController.Instance.HP > 0) Score += (int)PlayerController.Instance.HP;

            if (StageNum == 3)
            {
                UIManager.Instance.StageUIOn("GameClear!", Score, InputUserInfo);
            }
            else
            {
                UIManager.Instance.StageUIOn("StageClear!", Score, NextStage);
            }
        }
    }

    public void GameOver()
    {
        gameDirector.PlayerDie();
        UIManager.Instance.StageUIOn("GameOver!", Score, GoTitle);
    }

    private void GoTitle()
    {
        Destroy(UIManager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(this.gameObject);

        SceneManager.LoadScene(0);
    }

    private void NextStage()
    {
        StartCoroutine(NextStage());
        IEnumerator NextStage()
        {
            UIManager.Instance.StageUIOff();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(StageNum + 1);
            SetGameManager();
            gameDirector.StageStart(stageName[StageNum - 1]);
        }
    }

    private void InputUserInfo()
    {
        UIManager.Instance.InputFieldOn();
        var input = UIManager.Instance.inputField;

        StartCoroutine(InputUserInfo());
        IEnumerator InputUserInfo()
        {
            while (true)
            {
                if (input.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
                {
                    UserInfo n = new UserInfo();
                    n.userScore = Score;
                    n.userName = input.text;

                    JsonLoader.Save(n, "New_UserInfo");
                    break;
                }
                yield return null;
            }
            GoTitle();
        }
    }

}

public class UserInfo
{
    public int userScore;
    public string userName;
}
