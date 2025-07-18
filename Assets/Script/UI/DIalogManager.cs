using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] UIManager uiManager;
    public Image characterImage;
    public TextMeshProUGUI dialogText;
    public Button nextButton;
    [Header("For Debug")]
    [SerializeField] private DialogData tutorialDialog;

    private DialogData currentDialog;
    private int currentLine = 0;

    public void StartDialog(DialogData data)
    {
        currentDialog = data;
        currentLine = 0;
        uiManager.ShowDialogUI();
        ShowLine();
    }

    public void OnClickNext()
    {
        currentLine++;
        if (currentLine >= currentDialog.dialogLines.Length)
        {
            EndDialog();
        }
        else
        {
            ShowLine();
        }
    }

    private void ShowLine()
    {
        characterImage.sprite = currentDialog.characterSprite;
        dialogText.text = currentDialog.dialogLines[currentLine];
    }

    private void EndDialog()
    {
        //다이얼로그가 끝나면
        //uiManager.ShowInGameUI();
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
