using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingData
{
    public string nickname;
    public int score;

    public RankingData(string nickname, int score)
    {
        this.nickname = nickname;
        this.score = score;
    }
}


public class RankingManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject rankInputBg;
    public TMP_InputField nicknameInput;
    public GameObject rankListPanel;
    public Transform rankContentParent;
    public GameObject rankItemPrefab;

    private string currenNickname = "";
    private FirebaseFirestore db;
    private UIManager uiManager;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }
    public void PrepareNicknameInput()
    {
        if (nicknameInput != null)
        {
            nicknameInput.text = "익명";
            rankInputBg.SetActive(true);
        }

        if (rankListPanel != null)
        {
            rankListPanel.SetActive(false);
        }
    }


    public void SubmitScore(int score)
    {
        string nickname = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            return;
        }

        nickname = GenerateUniqueNickname(nickname);
        currenNickname = nickname;
        var data = new Dictionary<string, object>
        {
            { "nickname", nickname },
            { "score", score }
        };

        db.Collection("Ranking").Document(nickname).SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                rankInputBg.SetActive(false);
                LoadTopRanking();
            }
            else
            {
                Debug.LogError("Firestore 저장 실패: " + task.Exception);
            }
        });
    }


    public void LoadTopRanking()
    {
        // 기존 항목 제거
        foreach (Transform child in rankContentParent)
            Destroy(child.gameObject);

        db.Collection("Ranking")
          .GetSnapshotAsync()
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompletedSuccessfully)
              {
                  List<RankingData> allScores = new List<RankingData>();

                  foreach (var doc in task.Result.Documents)
                  {
                      string nickname = doc.GetValue<string>("nickname");
                      int score = doc.GetValue<int>("score");
                      allScores.Add(new RankingData(nickname, score));
                  }

                  // 클라이언트에서 직접 정렬
                  allScores.Sort((a, b) => b.score.CompareTo(a.score));
                  
                  ShowRankingList(allScores);
              }
          });
    }
    public void ShowRankingList(List<RankingData> sortedList)
    {
        rankListPanel.SetActive(true);
        foreach (Transform child in rankContentParent)
            Destroy(child.gameObject);

        int displayCount = Mathf.Min(10, sortedList.Count);
        int currentRank = 1;
        int realRank = 1;
        int? lastScore = null;

        for (int i = 0; i < displayCount; i++)
        {
            var data = sortedList[i];

            if (lastScore != null && data.score != lastScore)
                currentRank = realRank;

            GameObject item = Instantiate(rankItemPrefab, rankContentParent);
            item.GetComponentInChildren<TMP_Text>().text = $"{currentRank}등 : {data.nickname} / 점수: {data.score}";

            lastScore = data.score;
            realRank++;
        }
        int myScore = uiManager.GetScore();
        int myRank = GetMyRank(currenNickname, myScore, sortedList);

        uiManager.SetMyGrade(myRank);
        uiManager.SetGameOverInfo(currenNickname, myScore, myRank);
        uiManager.ShowGameOverUI();
    }


    public int GetMyRank(string myNickname, int myScore, List<RankingData> sortedList)
    {
        int rank = 1;
        foreach (var data in sortedList)
        {
            if (data.score > myScore)
            {
                rank++;
            }
            else if (data.score == myScore && data.nickname != myNickname)
            {
                rank++; // 동점자 앞에 있으면 계속 랭크 밀림
            }
            else if (data.nickname == myNickname && data.score == myScore)
            {
                break;
            }
        }

        Debug.Log($"{myNickname}님의 랭킹은 {rank}위입니다.");
        return rank;
    }
    public string GenerateUniqueNickname(string baseName)
    {
        string suffix = "#" + Random.Range(1000, 9999).ToString("X"); // 랜덤 16진 코드
        return $"{baseName}{suffix}";
    }

}
