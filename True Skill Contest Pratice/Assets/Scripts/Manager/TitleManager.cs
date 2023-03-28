using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject UIGroup;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private List<Button> buttons = new List<Button>();

    [SerializeField] private GameObject RankUI;
    [SerializeField] private List<Text> rankTexts = new List<Text>();
    [SerializeField] private List<UserInfo> rankUsers = new List<UserInfo>();
    private int max_rankUser = 5;

    private void Start()
    {
        buttons[0].onClick.AddListener(GameStart);
        buttons[1].onClick.AddListener(Ranking);
        buttons[2].onClick.AddListener(Exit);

        buttons[0].Select();
        RankLoad();
        NewUserCheck();
    }

    public void GameStart()
    {
        StartCoroutine(GameStart());

        IEnumerator GameStart()
        {
            float current = 0;
            float percent = 0;
            Vector3 start;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / 0.5f;
                start = playerObj.transform.position;

                playerObj.transform.position = Vector3.Lerp(start, new Vector3(30, 60, 80), percent);
                yield return null;
            }
            current = 0;
            while (current / 4f < 1)
            {
                current += Time.deltaTime;
                start = playerObj.transform.position;

                playerObj.transform.position = Vector3.Lerp(start, new Vector3(30, 60, 240), current / 4f);
                yield return null;
            }

            SceneManager.LoadScene(1);
            yield return null;
        }
    }

    public void Ranking()
    {
        rankUsers.Sort(RankSort);
        for (int i = 0; i < rankUsers.Count; i++)
        {
            if (i == rankTexts.Count) break;
            rankTexts[i].text = $"{i + 1}. {rankUsers[i].userName} : {rankUsers[i].userScore}";
        }

        RankUI.SetActive(!RankUI.activeSelf);
    }
    private int RankSort(UserInfo a, UserInfo b)
    {
        return a.userScore < b.userScore ? 1 : -1;
    }
    private void RankSave()
    {
        if (rankUsers.Count == 0) return;
        for (int i = 0; i < max_rankUser; i++)
            JsonLoader.Save(rankUsers[i], $"{i + 1}_UserInfo");
    }
    private void RankLoad()
    {
        for (int i = 0; i < max_rankUser; i++)
            rankUsers.Add(JsonLoader.Load<UserInfo>($"{i + 1}_UserInfo"));
    }
    private void NewUserCheck()
    {
        bool userCheck = false;

        var user = JsonLoader.Load<UserInfo>("New_UserInfo");
        foreach (var rankUsers in rankUsers)
        {
            if (rankUsers.userName.Equals(user.userName))
                userCheck = true;
        }

        if (!userCheck) rankUsers.Add(user);
    }

    public void Exit()
    {
        RankSave();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
