using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject inGamePanel;
    public GameObject dialogPanel;
    public GameObject gameOverPanel;

    [Header("UI Elements")]
    public Text scoreText;
    public Text livesText;


    private void Start()
    {
        tutorialPanel.SetActive(false);
        inGamePanel.SetActive(false);
        dialogPanel.SetActive(false);
        gameOverPanel.SetActive(false);

    }

    public void ShowTutorialUI() => SetPanel(tutorialPanel);
    public void ShowInGameUI() => SetPanel(inGamePanel);
    public void ShowDialogUI() => SetPanel(dialogPanel);
    public void ShowGameOverUI() => SetPanel(gameOverPanel);

    private void SetPanel(GameObject panelToActivate)
    {
        tutorialPanel.SetActive(false);
        inGamePanel.SetActive(false);
        dialogPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        panelToActivate.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = $"Lives: {lives}";
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
}