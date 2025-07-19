using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (bgmSlider != null && sfxSlider != null) {
            StartCoroutine(InitVolumeSliders());
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
