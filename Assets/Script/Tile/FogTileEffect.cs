using UnityEngine;
using System.Collections;

public class FogTileEffect : MonoBehaviour, ISpecialTile
{
    private float timeSinceStepped = 0f;
    private bool triggered = false;
    private TileComp tile;

    [SerializeField] private float fogTriggerTime = 5f;

    private void Awake()
    {
        tile = GetComponent<TileComp>();
    }

    private void OnEnable()
    {
        timeSinceStepped = 0f;
        triggered = false;
    }

    private void Update()
    {
        if (triggered || tile.GetTileType() != TileType.Fog) return;

        timeSinceStepped += Time.deltaTime;

        if (timeSinceStepped >= fogTriggerTime)
        {
            triggered = true;
            Debug.Log("FogTileEffect 발동");

            tile.tileManager.SpawnFogEffectOnRandomTile();

            tile.SetTileType(TileType.Normal);
        }
    }

    public void Activate(GameObject player)
    {
        timeSinceStepped = 0f; // 플레이어가 밟으면 초기화
    }

    public void ResetTile()
    {
        timeSinceStepped = 0f;
        triggered = false;
        StopAllCoroutines();
    }
}
