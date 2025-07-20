using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public Camera virtualCamera;

    [Header("UI Elements")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button nextButton;

    [Header("Dialog Data")]
    [SerializeField] private DialogData tutorialDialog;

    [SerializeField] private GameObject dialogUIGroup;
    [SerializeField] private ImageShaker warningImageShaker;

    private DialogData currentDialog;
    private int currentLine = 0;

    public void StartDialog()
    {
        Debug.Log("스토리 시작");
        currentDialog = tutorialDialog;
        currentLine = 0;
        uiManager.ShowDialogUI();
        dialogUIGroup.SetActive(false);
        nextButton.interactable = false;

        StartCoroutine(ShowLineWithBackgroundDelay());
    }

    public void OnClickNext()
    {
        currentLine++;
        if (currentLine == 2 || currentLine == 3) {
            warningImageShaker.Shake(15f, 0.5f);
            SoundManager.Instance.PlaySFX(SFXType.RockCrush);
        }
        if (currentLine >= currentDialog.dialogLines.Length)
        {
            EndDialog();
        }
        else
        {
            dialogUIGroup.SetActive(false);
            nextButton.interactable = false;
            StartCoroutine(ShowLineWithBackgroundDelay());
        }
    }

    private IEnumerator ShowLineWithBackgroundDelay()
    {
        dialogText.text = "";
        if (currentLine % 2 == 0)
        {
            int imgIndex = currentLine / 2;

            if (imgIndex < currentDialog.backgroundImages.Length)
                backgroundImage.sprite = currentDialog.backgroundImages[imgIndex];

            yield return new WaitForSeconds(2f);

            if (imgIndex < currentDialog.characterImages.Length)
                characterImage.sprite = currentDialog.characterImages[imgIndex];
        }

        dialogText.text = currentDialog.dialogLines[currentLine];
        dialogUIGroup.SetActive(true);

        nextButton.interactable = true;
        Debug.Log($"{currentLine + 1}번째 대사: {dialogText.text}");
    }


    private void EndDialog()
    {
        uiManager.ShowTutorialUI();
    }

    private void OnEnable()
    {
        nextButton.onClick.AddListener(OnClickNext);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(OnClickNext);
    }
}
