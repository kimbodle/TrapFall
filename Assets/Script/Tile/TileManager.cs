using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //언젠간 변수 프로퍼티로 보호해주기
    public GameObject tilePrefab;
    public int width = 5, height = 5; //일단 프로토타입에서는 5*5 
    public Vector2 spawnTilePoint; // 스폰을 시작할 좌표
    public float lastRandomTileSteppedTime = 0f;
    [SerializeField] private GameObject fogEffectPrefab;
    [SerializeField] private float randomTileCooldown = 3f;
    public bool enableRandomTile = false; // 라운드에 따라 설정
    private Coroutine randomMonitorCoroutine = null;


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
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x + spawnTilePoint.x, y + spawnTilePoint.y, 0), Quaternion.identity);
                TileComp tileComp = tileObj.GetComponent<TileComp>();
                tileComp.tileManager = this;
                tiles[x, y] = tileComp;
            }
        }
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

   

    private IEnumerator CheckRandomTriggerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (Time.time - lastRandomTileSteppedTime > randomTileCooldown)
            {
                TransformRandomTile();
                lastRandomTileSteppedTime = Time.time; // 리셋
            }
        }
    }


    public void StartRandomMonitor()
    {
        if (randomMonitorCoroutine != null) return;
        randomMonitorCoroutine = StartCoroutine(CheckRandomTriggerLoop());
    }

    public void StopRandomMonitor()
    {
        if (randomMonitorCoroutine != null)
        {
            StopCoroutine(randomMonitorCoroutine);
            randomMonitorCoroutine = null;
        }
    }



    public void SpawnFogTile()
    {
        var normalTiles = GetNormalTiles();
        if (normalTiles.Count == 0) return;

        TileComp target = normalTiles[Random.Range(0, normalTiles.Count)];

        if (fogEffectPrefab != null)
        {
            GameObject fog = Instantiate(fogEffectPrefab, target.transform.position, Quaternion.identity);
            Destroy(fog, 3f);
        }
    }
    public void SpawnFogEffectOnRandomTile()
    {
        List<TileComp> allTiles = tiles.Cast<TileComp>().ToList();
        if (allTiles.Count == 0) return;

        TileComp target = allTiles[Random.Range(0, allTiles.Count)];

        if (fogEffectPrefab != null)
        {
            GameObject fog = Instantiate(fogEffectPrefab, target.transform.position, Quaternion.identity);
            Destroy(fog, 3f);
        }
    }


    private void TransformRandomTile()
    {
        var randomTiles = GetTilesOfType(TileType.Random);
        if (randomTiles.Count == 0) return;

        TileComp target = randomTiles[Random.Range(0, randomTiles.Count)];

        TileType[] possibleTypes = new TileType[]
        {
        TileType.Spin,
        TileType.Ice,
        TileType.Trap,
        TileType.Fog
        };

        TileType chosen = possibleTypes[Random.Range(0, possibleTypes.Length)];
        target.SetTileType(chosen); // 여기서 자동으로 해당 TileEffect 붙음
    }

    public List<TileComp> GetTilesOfType(TileType type)
    {
        return tiles.Cast<TileComp>().Where(t => t.GetTileType() == type).ToList();
    }

}
