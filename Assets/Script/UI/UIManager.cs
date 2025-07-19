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
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverBestScoreText;

    [Header("UI Elements")]
    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI inGameTimeText;
    public TextMeshProUGUI inGameRoundText;

    [Header("Rank Elements")]
    public RankingManager rankingManager;

    private int currentScore = 0;


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
    public void ShowRankUI() => SetPanel(rankingPanel);

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

    public void UpdateScore(int score)
    {
        currentScore = score;

        if (gameOverScoreText != null) gameOverScoreText.text = $"{score}";
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

    public void OnClickStartGame()
    {
        //SceneLoader.Instance.StartGame();
    }

    public void OnClickReturnToMenu()
    {
        //SceneLoader.Instance.ReturnToMainMenu();
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
        ShowRankUI(); // 패널만 띄우고
        if (rankingManager != null)
            rankingManager.nicknameInput.text = "익명";
    }

    public void OnSubmitNickname()
    {
        if (rankingManager != null)
            rankingManager.SubmitScore(GetScore());
    }

}