using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    [SerializeField] Image TutorialImage;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button playButton;
    [SerializeField] Button closeButton;
    private int index = 0;

    public Sprite[] TutorialImages;

    void Start()
    {
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        TutorialImage.sprite = TutorialImages[index];
        UpdateUI();
    }
    
    void OnClickLeftButton()
    {
        if (index == 0) { return; }
        TutorialImage.sprite = TutorialImages[index - 1];
        index--;
        UpdateUI();
    }

    void OnClickRightButton()
    {
        if (index == TutorialImages.Length) { return; }
        TutorialImage.sprite = TutorialImages[index + 1];
        index++;
        UpdateUI();
    }
    void OnClickCloseButton()
    {
        //Todo: 게임 매니저 메소드 호출(인게임 UI띄우고 게임 시작하기)
        gameObject.SetActive(false);
    }
    private void UpdateUI()
    {
        switch (index)
        {
            case 0:
                leftButton.gameObject.SetActive(false);
                break;
            case 4:
                rightButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
                closeButton.gameObject.SetActive(false);
                break;
            default:
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);
                break;
        }
    }
}
