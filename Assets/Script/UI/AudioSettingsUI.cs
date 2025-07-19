using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    [SerializeField] private TextMeshProUGUI BGMTextVolume;
    [SerializeField] private TextMeshProUGUI SFXTextVolume;

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
        sfxSlider.value = SoundManager.Instance.GetBGMVolume();

        BGMTextVolume.text = $"{(int)(bgmSlider.value * 100)}";
        SFXTextVolume.text = $"{(int)(sfxSlider.value * 100)}";

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    //동적 연결
    private void SetBGMVolume(float value)
    {
        Debug.Log(value);
        SoundManager.Instance?.SetBGMVolume(value);
        BGMTextVolume.text = $"{(int)(value * 100)}";
    }

    //동적 연결
    private void SetSFXVolume(float value)
    {
        Debug.Log(value);
        SoundManager.Instance?.SetSFXVolume(value);
        SFXTextVolume.text = $"{(int)(value * 100)}";
    }
}
