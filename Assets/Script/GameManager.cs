using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField]
    private GameManager instance;
    public GameManager Instance {  get { return instance; } }

    public TileManager tileManager;
    public RoundCsvLoader roundCsvLoader;
    private List<RoundData> rounds = new List<RoundData>();
    private List<Coroutine> activeSpawnCoroutines = new List<Coroutine>();



    [SerializeField]
    int currentRound = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        InitRounds();
        StartCoroutine(RoundLoop());
    }

    void InitRounds()
    {
        //rounds.Add(new RoundData
        //{
        //    round = 1,
        //    tileSpawnRules = new List<TileSpawnRule>
        //    {
        //        new TileSpawnRule { tileType = TileType.Danger, spawnInterval = 1f }
        //    }
        //});

        //rounds.Add(new RoundData
        //{
        //    round = 2,
        //    tileSpawnRules = new List<TileSpawnRule>
        //    {
        //        new TileSpawnRule { tileType = TileType.Danger, spawnInterval = 1f },
        //        new TileSpawnRule { tileType = TileType.Spin, spawnInterval = 10f }
        //    }
        //});

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

    IEnumerator RoundLoop()
    {
        while (true)
        {
            currentRound++;
            Debug.Log($"[Round {currentRound}] 시작");

            RoundData roundData = rounds.FirstOrDefault(r => r.round == currentRound);
            if (roundData != null)
                StartRound(roundData);

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
                Coroutine co = StartCoroutine(SpawnTileLoop(rule.tileType, rule.spawnInterval));
                activeSpawnCoroutines.Add(co);
            }
        }
    }



    void EndRound()
    {
        foreach (var co in activeSpawnCoroutines)
            StopCoroutine(co);

        activeSpawnCoroutines.Clear();
    }
    IEnumerator SpawnTileLoop(TileType type, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            tileManager.SpawnSpecialTile(type);
        }
    }
}
