using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    //Todo: Ingamescene에서도 연결 필요
        //GameManager에서 현재 씬에 따라 브금 재생 필요
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (bgmSlider != null && sfxSlider != null) {
            StartCoroutine(InitVolumeSliders());
            if(SceneManager.GetActiveScene().name == "MainMenuScene")
            {
                SoundManager.Instance.PlayBGM(BGMType.MainMenu);
            }
        }
    }

    private IEnumerator InitVolumeSliders()
    {
        yield return new WaitUntil(() => SoundManager.Instance != null);
        
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    //동적 연결
    private void SetBGMVolume(float value)
    {
        SoundManager.Instance?.SetBGMVolume(value);
    }

    //동적 연결
    private void SetSFXVolume(float value)
    {
        SoundManager.Instance?.SetSFXVolume(value);
    }
}
