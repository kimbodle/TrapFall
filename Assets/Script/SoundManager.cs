using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public enum BGMType { Tutorial, Game}
    public enum SFXType { Jump,Walk,TileDestroy, RelicGet, GameOver,NextRound, BestScore }
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }


    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips, BGM")]
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

    private void SetupAudioSources()
    {
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(this.transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = 0.5f;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX_Source");
            sfxObj.transform.SetParent(this.transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.volume = 1.0f;
        }
    }

    public void PlaySFX(SFXType key)
    {
        if (sfxDict.TryGetValue(key, out var clip))
        {
            //타일파괴 사운드가 너무커서 볼륨을 줄임 나중에 오디오파일 자체를 수정할것
            sfxSource.volume = 1;
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
}
