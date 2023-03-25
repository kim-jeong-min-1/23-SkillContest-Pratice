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
    public PlayerController playerObject { get; set; } = null;

    private GameDirector gameDirector;
    [SerializeField] private List<Boss> bossPrefabs;
    [SerializeField] private List<string> stageNames;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }
    private void Start() => SceneManager.sceneLoaded += GameSceneLoaded;
    private void GameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetGameManager();
    }

    private void SetGameManager()
    {
        EnemySubject.Instance.enemyCount = 0;
        curStageNum++;

        qusetComplete = false;
        isCurStageClear = false;
        curStageBoss = null;

        playerObject = FindObjectOfType<PlayerController>();
        gameDirector = FindObjectOfType<GameDirector>();

        SetQuset();
        playerObject.PlayerReset();
        gameDirector.StageStart(playerObject.transform, stageNames[curStageNum - 1]);
    }

    private void SetQuset()
    {
        int conditon = curStageNum switch
        {
            1 => 50,
            2 => 80,
            3 => 100,
            _ => 0
        };
        qusetCondtion = conditon;
    }

    public void AddScore(int score) => stageScore += score;

    #region Button Listener
    private void NextStage()
    {      
        StartCoroutine(NextStageLoad());
        IEnumerator NextStageLoad()
        {
            UIManager.Instance.StageUIOff(1f);
            yield return new WaitForSecondsRealtime(1f);

            SceneManager.LoadScene(curStageNum + 1);
        }       
    }
    private void GoTitle()
    {
        SceneManager.LoadScene("Title");

        Destroy(playerObject.gameObject);
        Destroy(this.gameObject);
        Destroy(UIManager.Instance.gameObject);
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }

    private void InputUserInfo()
    {
        StartCoroutine(InputUserInfo());
        IEnumerator InputUserInfo()
        {
            UIManager.Instance.OnInputField();
            var inputField = UIManager.Instance.inputField;

            while (true)
            {
                if (inputField.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
                {
                    UserInfo user = new UserInfo();
                    user.userName = inputField.text;
                    user.userScore = stageScore;

                    JsonLoader.Save(user, "New_UserInfo");
                    break;
                }
                yield return null;
            }
            GoTitle();
        }
    }
    #endregion

    public void GameOver()
    {      
        gameDirector.PlayerDie(playerObject.transform);
        UIManager.Instance.SetStageUI("GameOver", stageScore, GoTitle);
        UIManager.Instance.StageUIOn(1f);
    }

    public void GameStopOnOff(bool isOnlyTime = false)
    {
        if (!isOnlyTime) 
        {
            PlayerController.Instance.enabled = !PlayerController.Instance.enabled;
            PlayerSkillSystem.Instance.enabled = !PlayerSkillSystem.Instance.enabled;
            if (curStageBoss) curStageBoss.enabled = !curStageBoss.enabled;
        }    
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }

    private void ClearCheck()
    {
        if (curStageBoss.isDie)
        {
            // 스테이지 클리어 처리
            isCurStageClear = true;
            gameDirector.BossDie(curStageBoss.transform);

            string text;
            if (curStageNum == 3)
            {
                text = "GameClear";
                UIManager.Instance.SetStageUI(text, stageScore, InputUserInfo);
            }
            else
            {
                text = "StageClear";
                stageScore += (int)PlayerController.Instance.Hp;
                UIManager.Instance.SetStageUI(text, stageScore, NextStage);
            }
            UIManager.Instance.StageUIOn(1f);
        }
    }

    private void SpawnBoss()
    {
        if (qusetComplete)
        {
            curStageBoss
            = Instantiate(bossPrefabs[curStageNum - 1], new Vector3(0, 0, 100f), Quaternion.identity);
            curStageBoss.HpMult(curStageNum);

            gameDirector.BossAppearance(curStageBoss.transform);
        }
    }

    private void Update()
    {
        UIManager.Instance.QusetUIUpdate($"Enemy {EnemySubject.Instance.enemyCount} / {qusetCondtion}");
        UIManager.Instance.ScoreUIUpdate($"Score : {stageScore}");

        if (!isCurStageClear)
        {
            if (EnemySubject.Instance.enemyCount == qusetCondtion) qusetComplete = true;

            if (curStageBoss == null) SpawnBoss();
            else ClearCheck();
        }


        //치트키
        if (Input.GetKeyDown(KeyCode.Escape)) PlayerController.Instance.isDie = !PlayerController.Instance.isDie;
        else if (Input.GetKeyDown(KeyCode.F1)) EnemySubject.Instance.DieAllEnemy();
        else if (Input.GetKeyDown(KeyCode.F2)) PlayerController.Instance.ShooterLevel= 4;
        else if (Input.GetKeyDown(KeyCode.F3)) PlayerSkillSystem.Instance.SkillCoolTimeInit();
        else if (Input.GetKeyDown(KeyCode.F4)) PlayerController.Instance.Hp = 200;
        else if (Input.GetKeyDown(KeyCode.F5)) PlayerController.Instance.Fuel = 1000;
        else if (Input.GetKeyDown(KeyCode.F6)) SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.F7)) SceneManager.LoadScene(2);
        else if (Input.GetKeyDown(KeyCode.F8)) SceneManager.LoadScene(3);
    }
}

public class UserInfo
{
    public string userName;
    public int userScore;
}

