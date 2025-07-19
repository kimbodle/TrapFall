using System.Collections.Generic;
using UnityEngine;

//타일 관련된 룰 
[System.Serializable]
public class TileSpawnRule
{
    public TileType tileType;
    public float spawnInterval;
}
//오브젝트 관련된 룰 
[System.Serializable]
public class ObjectSpawnRule
{
    public SpecialObjectType objectType;
    public float spawnInterval;
}
//아이템 관련된 룰 
[System.Serializable]
public class ItemSpawnRule
{
    public ItemType itemType;
    public float spawnInterval;
}
public enum TileType
{
    Normal = 0,
    Danger = 1,
    Spin = 2,
    Ice = 3,
    Trap = 4,
    Electric = 5,
    Fog = 6,
    Random = 7,
    Destroyed = 8, 
    Edge = 9
}
public enum SpecialObjectType
{
    EyeStoneWatcher,
    HowlingMouth
}
public enum ItemType
{
    TimeRelic,
    AncientRing,
    GoldenSunPiece,
    LostMapPiece
}
public enum CsvCol
{
    roundCol = 0,
    objectStart = 1, 
    objectEnd = 2,
    tileStart = 3,
    tileEnd = 9,
    itemStart = 10,
    itemEnd = 13
}
public class RoundCsvLoader : MonoBehaviour
{
    public TextAsset csvFile;

    public List<RoundData> LoadRoundsFromCSV()
    {
        var rounds = new List<RoundData>();
        string[] lines = csvFile.text.Split('\n');

        string[] headers = lines[0].Trim().Split(',');

        // 구간 정의

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cells = line.Split(',');
            RoundData rd = new RoundData();
            rd.round = int.Parse(cells[(int)CsvCol.roundCol]);

            // 🟢 Object 파싱
            for (int j = (int)CsvCol.objectStart; j <= (int)CsvCol.objectEnd; j++)
            {
                if (j >= cells.Length) continue;
                string value = cells[j].Trim();
                if (string.IsNullOrEmpty(value)) continue;

                if (float.TryParse(value, out float interval))
                {
                    try
                    {
                        var type = (SpecialObjectType)System.Enum.Parse(typeof(SpecialObjectType), headers[j], true);
                        rd.objectSpawnRules.Add(new ObjectSpawnRule
                        {
                            objectType = type,
                            spawnInterval = interval
                        });
                    }
                    catch { Debug.LogWarning($"Unknown object type: {headers[j]}"); }
                }
            }

            // 🟢 Tile 파싱
            for (int j = (int)CsvCol.tileStart; j <= (int)CsvCol.tileEnd; j++)
            {
                if (j >= cells.Length) continue;
                string value = cells[j].Trim();
                if (string.IsNullOrEmpty(value)) continue;

                if (float.TryParse(value, out float interval))
                {
                    try
                    {
                        var type = (TileType)System.Enum.Parse(typeof(TileType), headers[j], true);
                        rd.tileSpawnRules.Add(new TileSpawnRule
                        {
                            tileType = type,
                            spawnInterval = interval
                        });
                    }
                    catch { Debug.LogWarning($"Unknown tile type: {headers[j]}"); }
                }
            }

            // 🟢 Item 파싱
            for (int j = (int)CsvCol.itemStart; j <= (int)CsvCol.itemEnd; j++)
            {
                if (j >= cells.Length) continue;
                string value = cells[j].Trim();
                if (string.IsNullOrEmpty(value)) continue;

                if (float.TryParse(value, out float interval))
                {
                    try
                    {
                        var type = (ItemType)System.Enum.Parse(typeof(ItemType), headers[j], true);
                        rd.itemSpawnRules.Add(new ItemSpawnRule
                        {
                            itemType = type,
                            spawnInterval = interval
                        });
                    }
                    catch { Debug.LogWarning($"Unknown item type: {headers[j]}"); }
                }
            }

            rounds.Add(rd);
        }

        return rounds;
    }
}
