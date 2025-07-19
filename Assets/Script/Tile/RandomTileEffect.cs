using UnityEngine;
using System.Collections;

public class RandomTileEffect : MonoBehaviour, ISpecialTile
{
    private TileComp tile;
    private float timeSinceLastStepped = 0f;
    private bool transformed = false;

    [SerializeField] private float transformDelay = 3f;

    private void Awake()
    {
        tile = GetComponent<TileComp>();
    }

    private void OnEnable()
    {
        timeSinceLastStepped = 0f;
        transformed = false;
    }

    private void Update()
    {
        if (transformed || tile.GetTileType() != TileType.Random)
            return;

        timeSinceLastStepped += Time.deltaTime;

        if (timeSinceLastStepped >= transformDelay)
        {
            TransformToRandomTile();
            Debug.Log("RandomTileEffect 유후~");
        }
    }

    public void Activate(GameObject player)
    {
        timeSinceLastStepped = 0f;
    }

    private void TransformToRandomTile()
    {
        transformed = true;

        TileType[] options = new TileType[] {
            TileType.Spin, TileType.Ice, TileType.Trap, TileType.Fog
        };

        TileType selected = options[Random.Range(0, options.Length)];
        tile.SetTileType(selected); // 해당 타입으로 변신!
    }

    public void ResetTile()
    {
        transformed = false;
        timeSinceLastStepped = 0f;
        StopAllCoroutines();
    }
}
