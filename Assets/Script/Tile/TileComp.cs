using UnityEngine;
using System.Collections;

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
    Destroyed = 8
}

public interface ITimeEvent
{
    void OnPlayerEnter(GameObject player);
}

public class TileComp : MonoBehaviour
{
    public Sprite[] tileSprite;
    public float randomTileTime= 5.0f;
    public float recoveryTileTime = 3.0f;

    Vector2 worldPosition;
    [SerializeField]
    TileType currentTileType = TileType.Normal;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //밟았다고 치고
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsWalkable())
        {
            //일단은 타일에서 콜리전으로 플레이어 판단하기
            GameObject player = collision.gameObject;

            foreach (var tileEffect in GetComponents<ISpecialTile>())
            {
                tileEffect.Activate(player);
            }
        }
    }

    public bool IsWalkable()
    {
        //return currentTileType != TileType.Danger && currentTileType != TileType.Destroyed;
        return currentTileType != TileType.Destroyed;
    }
    public TileType GetTileType() { return currentTileType; }

    public void SetTileType(TileType inputTileType)
    {
        currentTileType = inputTileType;
        updateTileImage();
        ClearExistingEffects();
        // 랜덤으로 타일 바꾸기
        //if (currentTileType == TileType.Random) ChangeRandomTile();
        TileEvent();

        //if (currentTileType == TileType.Danger) StartCoroutine(DestroyTile());
    }

    public void ClearExistingEffects()
    {
        foreach (var effect in GetComponents<ISpecialTile>())
        {
            Destroy((Component)effect);
        }
    }

    private void updateTileImage()
    {
        int tileIndex = (int)currentTileType;
        if (!spriteRenderer || tileSprite.Length <= tileIndex || tileSprite[tileIndex] == null) return;

        spriteRenderer.sprite = tileSprite[tileIndex];
    }

    //셍성된 파괴된 타일은 시간이지나면 다시 원래대로 돌아옴
    IEnumerator DestroyTile()
    {
        yield return new WaitForSeconds(1f);
        SetTileType(TileType.Destroyed);

        yield return new WaitForSeconds(recoveryTileTime);
        SetTileType(TileType.Normal);
    }
    private void TileEvent()
    {
        switch (currentTileType)
        {
            case TileType.Danger:
                gameObject.AddComponent<DangerTileEffect>();
                StartCoroutine(DestroyTile());
                break;
            case TileType.Spin:
                gameObject.AddComponent<SpinTileEffect>();
                break;
                // Add other types here
        }
    }
}
