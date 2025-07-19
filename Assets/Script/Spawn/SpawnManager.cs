using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPrefabPair
{
    public ItemType itemType;
    public GameObject prefab;
}
public class SpawnManager : MonoBehaviour
{
    //프리펩 리스트
    public List<ItemPrefabPair> itemPrefabList;

    private Dictionary<ItemType, Queue<GameObject>> poolDict = new();
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
