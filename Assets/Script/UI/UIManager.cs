using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject inGamePanel;
    public GameObject dialogPanel;
    public GameObject gameOverPanel;
    public GameObject settingPanel;
    public GameObject rankingPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI NickNameText;

    [Header("UI Elements")]
    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI inGameTimeText;
    public TextMeshProUGUI inGameRoundText;

    [Header("Rank Elements")]
    public RankingManager rankingManager;

    private int currentScore = 0;
    private int myGrade = 0;


    private void Start()
    {
        tutorialPanel.SetActive(false);
        inGamePanel.SetActive(false);
        dialogPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingPanel.SetActive(false);
        rankingPanel.SetActive(false);
    }

    public void ShowTutorialUI() => SetPanel(tutorialPanel);
    public void ShowInGameUI() => SetPanel(inGamePanel);
    public void ShowDialogUI() => SetPanel(dialogPanel);
    public void ShowGameOverUI() => SetPanel(gameOverPanel);
    public void ShowRankUI() => ShowRankingProcess();
    private void SetPanel(GameObject panelToActivate)
    {
        tutorialPanel.SetActive(false);
        inGamePanel.SetActive(false);
        dialogPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingPanel.SetActive(false);
        rankingPanel.SetActive(false);

        panelToActivate.SetActive(true);
    }
    public void SetGameOverInfo(string nickname, int score, int rank)
    {
        if (gradeText != null)
            gradeText.text = $"{rank}등";

        if (NickNameText != null)
            NickNameText.text = $"{nickname}님의 점수:";
        if (gameOverScoreText != null)
            gameOverScoreText.text = $"{score} 점";
    }

    public void UpdateScore(int score)
    {
        currentScore = score;

        if (gradeText != null) gradeText.text = $"{myGrade}";
        if (inGameScoreText != null) inGameScoreText.text = $"{score}";
    }

    public void UpdateTime(float time)
    {
        if (inGameTimeText != null) inGameTimeText.text = $"{(int)time}";
    }
    public void UpdateRound(int round)
    {
        if (inGameRoundText != null) inGameRoundText.text = $"{round}";
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OnClickReStart()
    {
        SceneLoader.Instance.LoadScene("InGameScene");
    }

    public void OnClickMainMenu()
    {
        SceneLoader.Instance.LoadScene("MainMenuScene");
    }

    public void ShowRankingProcess()
    {
        SetPanel(rankingPanel);

        if (rankingManager != null)
            rankingManager.PrepareNicknameInput();
    }
    public void ShowTopRanking()
    {
        SetPanel(rankingPanel);

        if (rankingManager != null)
        {
            rankingManager.LoadTopRanking();
        }
            
    }

    public void OnSubmitNickname()
    {
        Debug.Log("OnSubmitNickname 호출됨");
        if (rankingManager != null)
        {
            Debug.Log("랭킹 매니저 SubmitScore 호출");
            rankingManager.SubmitScore(GetScore());
        }
    }

    public void SetMyGrade(int grade)
    {
        myGrade=grade;
    }

}