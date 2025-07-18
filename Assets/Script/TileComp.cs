using UnityEngine;

public enum TileType
{
    Normal   = 0,
    Danger   = 1,
    Spin     = 2,
    Ice      = 3,
    Trap     = 4,
    Electric = 5,
    Fog      = 6,
    Random   = 7,
    Destroyed = -1
}

public class TileComp : MonoBehaviour
{
    public Sprite[] tileSprite;
    public float RandomTileTime= 5.0f;

    Vector2 worldPosition;
    [SerializeField]
    TileType currentTileType = TileType.Normal;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsWalkable())
        {
            //일단은 타일에서 콜리전으로 플레이어 판단하기
            Debug.Log(GetTileType().ToString());
            SetTileType(TileType.Danger);
        }
    }

    void ChangeRandomTile()
    {
        int tileIndex = Random.Range(0, tileSprite.Length - 1);
        SetTileType((TileType)tileIndex);
    }
    public bool IsWalkable()
    {
        return currentTileType != TileType.Danger && currentTileType != TileType.Destroyed;
    }
    public TileType GetTileType() { return currentTileType; }

    public void SetTileType(TileType intputTileType)
    {
        currentTileType = intputTileType;
        // 랜덤으로 타일 바꾸기
        if (currentTileType == TileType.Random) ChangeRandomTile();
        updateTileImage();
    }
    private void updateTileImage()
    {
        int tileIndex = (int)currentTileType;

        if (!spriteRenderer || !tileSprite[tileIndex]) return;
        spriteRenderer.sprite = tileSprite[tileIndex];
    }
}
