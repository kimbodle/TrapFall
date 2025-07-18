using System.Collections.Generic;
using UnityEngine;

public class RoundCsvLoader : MonoBehaviour
{
    public TextAsset csvFile;

    public List<RoundData> LoadRoundsFromCSV()
    {
        var rounds = new List<RoundData>();
        string[] lines = csvFile.text.Split('\n');

        string[] headers = lines[0].Trim().Split(','); // 첫 줄: 타일 이름

        const int tileTypeStartIndex = 3;
        const int tileTypeEndIndex = 9;

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            //Debug.Log(line);
            if (string.IsNullOrEmpty(line)) continue;

            string[] cells = line.Split(',');

            RoundData rd = new RoundData();
            rd.round = int.Parse(cells[0]);
            rd.tileSpawnRules = new List<TileSpawnRule>();


            for (int j = tileTypeStartIndex; j <= tileTypeEndIndex; j++)
            {
                if (j >= cells.Length) continue;

                string value = cells[j].Trim();
                if (string.IsNullOrEmpty(value)) continue;

                if (float.TryParse(value, out float interval))
                {
                    // 열 이름을 enum과 매칭
                    try
                    {
                        TileType type = (TileType)System.Enum.Parse(typeof(TileType), headers[j], ignoreCase: true);
                        rd.tileSpawnRules.Add(new TileSpawnRule
                        {
                            tileType = type,
                            spawnInterval = interval
                        });
                    }
                    catch
                    {
                        Debug.LogWarning($"⚠️ Unknown tile type in CSV: {headers[j]} (line {i + 1})");
                    }
                }
            }

            rounds.Add(rd);
        }

        return rounds;
    }
}
