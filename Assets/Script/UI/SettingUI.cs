using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    //Todo: mainmenu 씬과 Ingame 씬 구별해서 UI 메소드 구현해야함
            // 확인/취소 -> 메인메뉴/계속하기
    [SerializeField] private Button settingButton;
    [SerializeField] private GameObject settingPanel;
    void Start()
    {
        settingButton.onClick.AddListener(ActiveSettingUI);
    }

    public void ActiveSettingUI()
    {
        settingPanel.SetActive(true);
    }

    public void DeactiveSettingUI()
    {
        settingPanel.SetActive(false);
    }

}
