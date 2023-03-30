using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int curStageNum { get; private set; } = 0;
    public int quest { get; private set; } = 0;
    public int score { get; set; } = 0;

    private void Awake()
    {
        SetInstance();
        SetGameManager();
    }

    private void SetGameManager()
    {
        curStageNum++;
        SetQuest(curStageNum);
    }

    private void SetQuest(int stageNum)
    {
        quest = stageNum switch
        {
            1 => 50,
            2 => 80,
            3 => 100,
            _ => 0
        };
    }
}
