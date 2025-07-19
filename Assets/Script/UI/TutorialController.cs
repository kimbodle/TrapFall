using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    [SerializeField] Image TutorialImage;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button closeButton;
    private int index = 0;

    public Sprite[] TutorialImages;

    void Start()
    {
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        TutorialImage.sprite = TutorialImages[index];
    }
    
    void OnClickLeftButton()
    {
        if (index == 0) { return; }
        TutorialImage.sprite = TutorialImages[index - 1];
        index--;

    }

    void OnClickRightButton()
    {
        if (index == 2) { return; }
        TutorialImage.sprite = TutorialImages[index + 1];
        index++;
    }
    void OnClickCloseButton()
    {
        //Todo: 게임 매니저 메소드 호출(인게임 UI띄우고 게임 시작하기)
        gameObject.SetActive(false);
    }
}
