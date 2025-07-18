using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 5, height = 5; //일단 프로토타입에서는 5*5 
    public Vector2 spawnTilePoint; // 스폰을 시작할 좌표
    //public float 

    TileComp[,] tiles;
    public List<TileType> roundTileTypes = new List<TileType>();

    void Start()
    {
        //게임 시작시 타일 생성
        GeneratedGrid();
    }

    void GeneratedGrid()
    {
        tiles = new TileComp[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x + spawnTilePoint.x, y + spawnTilePoint.y, 0), Quaternion.identity);
                tiles[x,y] = tileObj.GetComponent<TileComp>();
            }
        }
    }

    public void ApplySpecialTiles(int round)
    {
        // 라운드별 타일 수 증가 (ex: 3개, 6개, 9개...)
        int specialCount = Mathf.Min(round * 3, width * height);

        List<TileComp> flatTiles = tiles.Cast<TileComp>().ToList();
        List<TileComp> normalTiles = flatTiles.Where(t => t.GetTileType() == TileType.Normal).ToList();

        var selectedTiles = normalTiles.OrderBy(x => Random.value).Take(specialCount);

        foreach (TileComp tile in selectedTiles)
        {
            //확률표에 따라 특수파일을 소환
            // 라운드별 가능한 특수타일 중 랜덤 선택
            //TileType chosen = roundTileTypes[Random.Range(0, roundTileTypes.Count)];
            TileType chosen = TileType.Spin;
            tile.SetTileType(chosen);
        }

        Debug.Log($"[Round {round}] {specialCount} 타일이 특수타일로 변경됨");
    }

    TileComp GetTile(Vector2Int coord)
    {
        if(0 < coord.x && coord.x < width && 0 < coord.y && coord.y < height)
        {
            return tiles[coord.x,coord.y];
        }
        return null;
    }

    public void SpawnSpecialTile(TileType type)
    {
        List<TileComp> normalTiles = GetNormalTiles();
        if (normalTiles.Count == 0) return;

        TileComp randomTile = normalTiles[Random.Range(0, normalTiles.Count)];
        randomTile.SetTileType(type);
    }

    public List<TileComp> GetNormalTiles()
    {
        return tiles.Cast<TileComp>().Where(t => t.GetTileType() == TileType.Normal).ToList();
    }
}
