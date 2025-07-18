using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameManager instance;
    public GameManager Instance {  get { return instance; } }

    public TileManager tileManager;

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
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            currentRound++;

            // 라운드마다 가능한 특수타일 종류 설정
            tileManager.roundTileTypes = GetAllowedTileTypesForRound(currentRound);

            tileManager.ApplySpecialTiles(currentRound);
        }
    }
    private List<TileType> GetAllowedTileTypesForRound(int round)
    {
        // 예시: 라운드 증가에 따라 더 많은 종류 허용
        if (round == 1) return new List<TileType> { TileType.Danger };
        if (round == 2) return new List<TileType> { TileType.Danger, TileType.Ice };
        if (round == 3) return new List<TileType> { TileType.Danger, TileType.Ice, TileType.Trap };
        return new List<TileType>
        {
            TileType.Danger,
            TileType.Ice,
            TileType.Spin,
            TileType.Trap,
            TileType.Electric,
            TileType.Fog,
            TileType.Random
        };
    }
}
