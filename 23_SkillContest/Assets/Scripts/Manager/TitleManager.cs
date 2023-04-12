using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<Text> rankText;
    [SerializeField] private GameObject RankUI;
    [SerializeField] private GameObject GameExplain;

    private List<UserInfo> ranks = new List<UserInfo>();
    private Animator titleAnimator => GetComponent<Animator>();

    void Start()
    {
        Load();
        NewRank();

        buttons[0].onClick.AddListener(GameStart);
        buttons[1].onClick.AddListener(Rank);
        buttons[2].onClick.AddListener(Exit);

        buttons[0].Select();
        SoundManager.Instance.PlayBgm(Bgm.Title, 1f);
    }

    private void GameStart()
    {        
        StartCoroutine(StageStart());
        IEnumerator StageStart()
        {
            SoundManager.Instance.PlaySfx(SoundEffect.Button1, 1f);
            GameExplain.SetActive(true);
            yield return new WaitForSeconds(0.2f);

            while (!Input.GetKeyDown(KeyCode.Return)) yield return null;
            GameExplain.SetActive(false);
            yield return new WaitForSeconds(0.2f);

            titleAnimator.SetTrigger("GameStart");
            for (int i = 0; i < buttons.Count; i++) buttons[i].onClick.RemoveAllListeners();

            yield return new WaitForSeconds(4.8f);
            SceneManager.LoadScene(1);
        }
    }

    private void Rank()
    {
        SoundManager.Instance.PlaySfx(SoundEffect.Button1, 1f);
        ranks.Sort(RankSort);
        for (int i = 0; i < rankText.Count; i++)
        {
            rankText[i].text = $"{ranks[i].userName} : {ranks[i].userScore}";
        }

        RankUI.SetActive(!RankUI.activeSelf);
        Save();
    }

    private int RankSort(UserInfo a, UserInfo b)
    {
        return (a.userScore < b.userScore) ? 1 : -1;
    }

    private void NewRank()
    {
        UserInfo n = JsonLoader.Load<UserInfo>("New_UserInfo");

        foreach (var rank in ranks)
        {
            if (rank == null) continue;
            if (n.userName.Equals(rank.userName))
            {
                return;
            }
        }
        ranks.Add(n);
    }

    private void Save()
    {
        ranks.Sort(RankSort);
        for (int i = 0; i < rankText.Count; i++)
            JsonLoader.Save(ranks[i], $"Rank_{i + 1}");
    }

    private void Load()
    {
        for (int i = 0; i < rankText.Count; i++)
            ranks.Add(JsonLoader.Load<UserInfo>($"Rank_{i + 1}"));
    }


    private void Exit()
    {
        SoundManager.Instance.PlaySfx(SoundEffect.Button1, 1f);
        Save();
#if UNITY_EDITOR
        return;
#else
    Application.Quit();
#endif
    }

}
