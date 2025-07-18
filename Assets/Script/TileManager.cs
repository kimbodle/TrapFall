using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 5, height = 5; //일단 프로토타입에서는 5*5 
    public Vector2 spawnTilePoint; // 스폰을 시작할 좌표
    //public float 

    TileComp[,] tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    TileComp GetTile(Vector2Int coord)
    {
        if(0 < coord.x && coord.x < width && 0 < coord.y && coord.y < height)
        {
            return tiles[coord.x,coord.y];
        }
        return null;
    }
}
