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

    private float originalBGMVolume;
    private float originalSFXVolume;

    private void Start()
    {
        StartCoroutine(InitVolumeSliders());
    }

    private IEnumerator InitVolumeSliders()
    {
        yield return new WaitUntil(() => SoundManager.Instance != null);

        originalBGMVolume = SoundManager.Instance.GetBGMVolume();
        originalSFXVolume = SoundManager.Instance.GetSFXVolume();

        bgmSlider.value = originalBGMVolume;
        sfxSlider.value = originalSFXVolume;

        UpdateVolumeText();

        bgmSlider.onValueChanged.AddListener(OnPreviewBGMVolume);
        sfxSlider.onValueChanged.AddListener(OnPreviewSFXVolume);
    }

    private void OnPreviewBGMVolume(float value)
    {
        BGMTextVolume.text = $"{(int)(value * 100)}";
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void OnPreviewSFXVolume(float value)
    {
        SFXTextVolume.text = $"{(int)(value * 100)}";
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void OnClickApply()
    {
        originalBGMVolume = bgmSlider.value;
        originalSFXVolume = sfxSlider.value;
    }

    public void OnClickCancel()
    {
        bgmSlider.value = originalBGMVolume;
        sfxSlider.value = originalSFXVolume;

        SoundManager.Instance.SetBGMVolume(originalBGMVolume);
        SoundManager.Instance.SetSFXVolume(originalSFXVolume);

        UpdateVolumeText();
    }

    private void UpdateVolumeText()
    {
        BGMTextVolume.text = $"{(int)(bgmSlider.value * 100)}";
        SFXTextVolume.text = $"{(int)(sfxSlider.value * 100)}";
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
