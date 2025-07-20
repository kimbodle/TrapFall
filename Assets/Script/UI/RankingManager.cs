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

    private bool isSubmitted = false;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    private void Start()
    {
        uiManager = GetComponent<UIManager>();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase �ʱ�ȭ �Ϸ��");
            }
            else
            {
                Debug.LogError($"Firebase �ʱ�ȭ ����: {result}");
            }
        });
    }
    public void PrepareNicknameInput()
    {
        if (nicknameInput != null)
        {
            nicknameInput.text = "�͸�";
            rankInputBg.SetActive(true);
        }

        if (rankListPanel != null)
        {
            rankListPanel.SetActive(false);
        }
    }


    public void SubmitScore(int score)
    {
        if (isSubmitted) return; // �ߺ� ���� ����

        string nickname = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nickname)) return;

        nickname = GenerateUniqueNickname(nickname);
        currenNickname = nickname;

        var data = new Dictionary<string, object>
    {
        { "nickname", nickname },
        { "score", score }
    };

        isSubmitted = true;

        db.Collection("Ranking").Document(nickname).SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                rankInputBg.SetActive(false);
                LoadTopRanking();
            }
            else
            {
                Debug.LogError("Firestore ���� ����: " + task.Exception);
                isSubmitted = false; // ���� �� �ٽ� ���� ���
            }
        });
    }


    public void LoadTopRanking()
    {
        // ���� �׸� ����
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

                  // Ŭ���̾�Ʈ���� ���� ����
                  allScores.Sort((a, b) => b.score.CompareTo(a.score));
                  
                  ShowRankingList(allScores);
              }
              else
              {
                  Debug.Log("IsCompletedSuccessfully ����");
              }
          });
    }
    public void ShowRankingList(List<RankingData> sortedList)
    {
        rankListPanel.SetActive(true);
        foreach (Transform child in rankContentParent)
            Destroy(child.gameObject);

        int displayCount = Mathf.Min(100, sortedList.Count);
        int currentRank = 1;
        int realRank = 1;
        int? lastScore = null;

        for (int i = 0; i < displayCount; i++)
        {
            var data = sortedList[i];

            if (lastScore != null && data.score != lastScore)
                currentRank = realRank;

            GameObject item = Instantiate(rankItemPrefab, rankContentParent);
            Transform rankText = item.transform.GetChild(0); // ù ��° �ڽ�
            Transform nicknameText = item.transform.GetChild(1); // �� ��° �ڽ�

            rankText.GetComponent<TMP_Text>().text = $"{currentRank}";
            nicknameText.GetComponent<TMP_Text>().text = $"{data.nickname} / ����: {data.score}";


            lastScore = data.score;
            realRank++;
        }
        int myScore = uiManager.GetScore();
        int myRank = GetMyRank(currenNickname, myScore, sortedList);

        uiManager.SetMyGrade(myRank);
        uiManager.SetGameOverInfo(currenNickname, myScore, myRank);
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
                rank++; // ������ �տ� ������ ��� ��ũ �и�
            }
            else if (data.nickname == myNickname && data.score == myScore)
            {
                break;
            }
        }

        Debug.Log($"{myNickname}���� ��ŷ�� {rank}���Դϴ�.");
        return rank;
    }
    public string GenerateUniqueNickname(string baseName)
    {
        string suffix = "#" + Random.Range(1000, 9999).ToString("X"); // ���� 16�� �ڵ�
        return $"{baseName}{suffix}";
    }

}
