using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class RoundData
{
    public int round;
    public List<TileSpawnRule> tileSpawnRules = new();
    public List<ObjectSpawnRule> objectSpawnRules = new();
    public List<ItemSpawnRule> itemSpawnRules = new();
}

public class GameManager : MonoBehaviour
{
    //Todo: 사운드를 위해 InGameScene -> MainMenu로 옮겨야함(추후 점수데이터와 유저 정보에도 필요)
    public static GameManager Instance { get; private set; }

    public TileManager tileManager;
    public RoundCsvLoader roundCsvLoader;
    public UIManager uiManager;
    public SpawnManager spawnManager;

    private List<RoundData> rounds = new List<RoundData>();
    private List<Coroutine> activeSpawnCoroutines = new List<Coroutine>();

    private bool isGameOver = false;

    [SerializeField]
    int currentRound = 0;
    int currentScore = 0;
    float currentTime = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        uiManager = GetComponentInChildren<UIManager>();
    }

    private void Start()
    {
        InitRounds();
        //게임 시작 함수
        GameStart();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        uiManager.UpdateTime(currentTime);
    }

    void InitRounds()
    {
        rounds = roundCsvLoader.LoadRoundsFromCSV();
        //for (int i = 0; i < rounds.Count; i++)
        //{
        //    Debug.Log(rounds[i].round + "라운드 데이터 ");
        //    for (int j = 0; j < rounds[i].tileSpawnRules.Count; j++)
        //    {
        //        TileSpawnRule rule = rounds[i].tileSpawnRules[j];
        //        Debug.Log(rounds[i].round + "라운드 데이터 " + rule.tileType + "번째 타일 형태" + rule.spawnInterval);

        //    }
        //}
    }
    public void GameStart()
    {
        SoundManager.Instance.PlayBGM(BGMType.Game);
        uiManager.ShowInGameUI();
        StartCoroutine(RoundLoop());
        StartCoroutine(CheckTime());
    }
    IEnumerator CheckTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SetScore(1);
        }
    }


    IEnumerator RoundLoop()
    {
        while (true)
        {
            currentRound++;
            uiManager.UpdateRound(currentRound);
            Debug.Log($"[Round {currentRound}] 시작");

            RoundData roundData = rounds.FirstOrDefault(r => r.round == currentRound);
            if (roundData != null)
            {
                SoundManager.Instance.PlaySFX(SFXType.NextRound);
                StartRound(roundData);
            }

            yield return new WaitForSeconds(10f); // 라운드 진행 시간
            EndRound(); // 다음 라운드 전 준비
        }
    }

    void StartRound(RoundData data)
    {
        foreach (var rule in data.tileSpawnRules)
        {
            if (rule.spawnInterval > 0)
            {
                Coroutine co = StartCoroutine(SpawnLoop<TileType>(rule.tileType, rule.spawnInterval));
                activeSpawnCoroutines.Add(co);
            }
        }

        foreach (var rule in data.objectSpawnRules)
        {
            if (rule.spawnInterval > 0)
            {
                Coroutine co = StartCoroutine(SpawnLoop<SpecialObjectType>(rule.objectType, rule.spawnInterval));
                activeSpawnCoroutines.Add(co);
            }
        }

        foreach (var rule in data.itemSpawnRules)
        {
            if (rule.spawnInterval > 0)
            {
                Coroutine co = StartCoroutine(SpawnLoop<ItemType>(rule.itemType, rule.spawnInterval));
                activeSpawnCoroutines.Add(co);
            }
        }
    }

    void EndRound()
    {
        foreach (var co in activeSpawnCoroutines)
            StopCoroutine(co);

        activeSpawnCoroutines.Clear();
        spawnManager.ResetSpawnedObject();
    }
    IEnumerator SpawnLoop<T>(T type, float interval) where T : Enum
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (typeof(T) == typeof(TileType))
            {
                tileManager.SpawnSpecialTile((TileType)(object)type);
            }
            else if (typeof(T) == typeof(ItemType))
            {
                spawnManager.SpawnItem((ItemType)(object)type);
            }
            else if (typeof(T) == typeof(SpecialObjectType))
            {
                //spawnManager.SpawnObject((SpecialObjectType)(object)type);
            }
            else
            {
                Debug.LogWarning($"Unhandled type {typeof(T)}");
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("💀 Game Over!");
        // UI, 재시작 등 호출
        //그거 검정 페이드인 카메라 연출
        //게임 오버 UI출력

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX(SFXType.GameOver);
        EndRound() ;
        StopAllCoroutines();
        tileManager.ResetTiles();
        uiManager.ShowGameOverUI();
    }

    public void SetScore(int score)
    {
        currentScore += score;
        uiManager.UpdateScore(currentScore);
        Debug.Log($"{score}");
    }
}
