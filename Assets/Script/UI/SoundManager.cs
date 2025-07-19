using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMType { MainMenu, Tutorial, Game }
public enum SFXType { Jump, Walk, TileDestroy, RelicGet, GameOver, NextRound, BestScore }
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }


    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips, BGM")]
    public AudioClip bgmMainMenu;
    public AudioClip bgmTutorial;
    public AudioClip bgmGame;

    [Header("Audio Clips, BGM")]
    public AudioClip sfxJump;
    public AudioClip sfxWalk;
    public AudioClip sfxTileDestroy;
    public AudioClip sfxRelicGet;
    public AudioClip sfxNextRound;
    public AudioClip sfxGameOver;
    public AudioClip sfxBestScore;

    [SerializeField][Range(0f, 1f)] private float defaultBGMVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float defaultSFXVolume = 1f;


    private Dictionary<BGMType, AudioClip> BGMDict = new();
    private Dictionary<SFXType, AudioClip> sfxDict = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupAudioSources();
        BGMDict[BGMType.MainMenu] = bgmMainMenu;
        BGMDict[BGMType.Tutorial] = bgmTutorial;
        BGMDict[BGMType.Game] = bgmGame;

        // 클립 등록 (enum으로)
        sfxDict[SFXType.Jump] = sfxJump;
        sfxDict[SFXType.Walk] = sfxWalk;
        sfxDict[SFXType.TileDestroy] = sfxTileDestroy;
        sfxDict[SFXType.RelicGet] = sfxRelicGet;
        sfxDict[SFXType.GameOver] = sfxGameOver;
        sfxDict[SFXType.NextRound] = sfxNextRound;
        sfxDict[SFXType.BestScore] = sfxBestScore;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenuScene":
                PlayBGM(BGMType.MainMenu);
                break;
            case "InGameScene":
                PlayBGM(BGMType.Tutorial);
                //튜토리얼 판넬 클로즈 버튼 누르먄 인게임 브금 재생하기
                break;
            default:
                Debug.LogWarning($"[SoundManager] 알 수 없는 씬 이름: {scene.name}");
                break;
        }
    }



    private void SetupAudioSources()
    {
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(this.transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX_Source");
            sfxObj.transform.SetParent(this.transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        bgmSource.volume = defaultBGMVolume;
        sfxSource.volume = defaultSFXVolume;
    }


    public void PlaySFX(SFXType key)
    {
        if (sfxDict.TryGetValue(key, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX '{key}' 없음!");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(BGMType key, bool loop = true)
    {
        if (BGMDict.TryGetValue(key, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX '{key}' 없음!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void SetBGMVolume(float volume)
    {
        if (bgmSource == null) { return; }
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource == null) { return; }
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    public float GetBGMVolume() => bgmSource.volume;
    public float GetSFXVolume() => sfxSource.volume;


}
