using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPrefabPair
{
    public ItemType itemType;
    public GameObject prefab;
}

[System.Serializable]
public class ObjectPrefabPair
{
    public SpecialObjectType objectType;
    public GameObject prefab;
}
public class SpawnManager : MonoBehaviour
{
    //프리펩 리스트
    public List<ItemPrefabPair> itemPrefabList;
    public List<ObjectPrefabPair> objectPrefabList;

    private Dictionary<ItemType, Queue<GameObject>> poolDict = new();
    private Dictionary<SpecialObjectType, Queue<GameObject>> poolObjDict = new();
    private const int POOL_SIZE = 4;

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        foreach (var pair in itemPrefabList)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < POOL_SIZE; i++)
            {
                GameObject go = Instantiate(pair.prefab, transform);
                go.SetActive(false);
                queue.Enqueue(go);
            }

            poolDict[pair.itemType] = queue;
        }

        foreach (var pair in objectPrefabList)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < POOL_SIZE; i++)
            {
                GameObject go = Instantiate(pair.prefab, transform);
                go.SetActive(false);
                queue.Enqueue(go);
            }

            poolObjDict[pair.objectType] = queue;
        }
    }

    public void SpawnItem(ItemType itemType)
    {
        if (!poolDict.ContainsKey(itemType))
        {
            Debug.LogWarning($"[ItemManager] ItemType {itemType} 풀에 없음");
            return;
        }

        List<TileComp> normalTiles = GameManager.Instance.tileManager.GetNormalTiles();
        if (normalTiles.Count == 0) return;

        TileComp tile = normalTiles[Random.Range(0, normalTiles.Count)];
        Vector3 spawnPos = tile.transform.position + new Vector3(0,0.5f,0);

        GameObject item = poolDict[itemType].Dequeue();
        item.transform.position = spawnPos;
        item.SetActive(true);

        poolDict[itemType].Enqueue(item); // 다시 뒤로 넣어서 순환
    }
    public void SpawnObject(SpecialObjectType objectType)
    {
        if (!poolObjDict.ContainsKey(objectType))
        {
            Debug.LogWarning($"[ItemManager] ItemType {objectType} 풀에 없음");
            return;
        }

        List<TileComp> edgeTiles = GameManager.Instance.tileManager.GetEdgeTiles();
        if (edgeTiles.Count == 0) return;

        TileComp selected = edgeTiles[Random.Range(0, edgeTiles.Count)];
        Vector3 spawnPos = selected.transform.position;
        Vector2 shootDir = GetShootDirectionFromTile(selected);

        GameObject obj = poolObjDict[objectType].Dequeue();
        obj.transform.position = spawnPos;
        obj.SetActive(true);

        // 방향 지정
        if (obj.TryGetComponent<StatueWatcher>(out var watcher))
        {
            watcher.SetDirection(shootDir);
        }

        poolObjDict[objectType].Enqueue(obj);
    }
    private Vector2 GetShootDirectionFromTile(TileComp tile)
    {
        Vector2Int coord = tile.nodePosition; // 타일에 위치 정보가 있어야 함
        int width = GameManager.Instance.tileManager.width;
        int height = GameManager.Instance.tileManager.height;

        if (coord.x == 0) return Vector2.right;
        if (coord.x == width - 1) return Vector2.left;
        if (coord.y == 0) return Vector2.up;
        if (coord.y == height - 1) return Vector2.down;

        return Vector2.down; // 기본값
    }

    public void ResetSpawnedObject()
    {
        foreach (var pair in poolDict) // Dictionary<ItemType, Queue<GameObject>>
        {
            foreach (var obj in pair.Value)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
