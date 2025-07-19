using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuRankingViewer : MonoBehaviour
{
    [Header("UI")]
    public GameObject rankListPanel;
    public Transform rankContentParent;
    public GameObject rankItemPrefab;
    public Button rankButton;
    public Button closeButton;

    private FirebaseFirestore db;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    private void Start()
    {
        rankButton.onClick.AddListener(LoadRankingOnly);
        closeButton.onClick.AddListener(DeactiverankListPanel);
        rankListPanel.SetActive(false);
    }

    public void DeactiverankListPanel()
    {
        rankListPanel.SetActive(false);
    }
    public void LoadRankingOnly()
    {
        db.Collection("Ranking").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompletedSuccessfully)
            {
                Debug.LogError("랭킹 불러오기 실패");
                return;
            }

            List<RankingData> allScores = new List<RankingData>();
            foreach (var doc in task.Result.Documents)
                allScores.Add(new RankingData(doc.GetValue<string>("nickname"), doc.GetValue<int>("score")));

            allScores.Sort((a, b) => b.score.CompareTo(a.score));
            ShowRankingListOnly(allScores);
        });
    }

    private void ShowRankingListOnly(List<RankingData> sortedList)
    {
        rankListPanel.SetActive(true);
        foreach (Transform child in rankContentParent)
            Destroy(child.gameObject);

        int currentRank = 1;
        int realRank = 1;
        int? lastScore = null;

        for (int i = 0; i < Mathf.Min(10, sortedList.Count); i++)
        {
            var data = sortedList[i];

            if (lastScore != null && data.score != lastScore)
                currentRank = realRank;

            GameObject item = Instantiate(rankItemPrefab, rankContentParent);
            item.GetComponentInChildren<TMP_Text>().text = $"{currentRank}등 : {data.nickname} / 점수: {data.score}";

            lastScore = data.score;
            realRank++;
        }
    }
}
