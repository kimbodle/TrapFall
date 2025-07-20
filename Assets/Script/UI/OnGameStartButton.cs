using UnityEngine;
using UnityEngine.UI;

public class OnGameStartButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onClickStartButton);
    }
    
    public void onClickStartButton()
    {
        SceneLoader.Instance.LoadScene("InGameScene");
    }
}
